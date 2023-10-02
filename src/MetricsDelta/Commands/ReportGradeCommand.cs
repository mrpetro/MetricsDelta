using MetricsDelta.Configuration;
using MetricsDelta.Extensions;
using MetricsDelta.Helpers;
using MetricsDelta.Model.Xml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;

namespace MetricsDelta.Commands
{
    public class ReportGradeCommand : Command
    {
        #region Private Constructors

        private ReportGradeCommand(string name, string? description = null) : base(name, description)
        {
        }

        #endregion Private Constructors

        #region Public Methods

        public static ReportGradeCommand Create(string[] args)
        {
            var command = new ReportGradeCommand("grade", "Grade given code metrics report");

            var metricsFilePathOption = new Option<string>
                (name: "--metricsFilePath",
                description: "Path to the code metrics report file.");

            var reportFilePathOption = new Option<string>
                (name: "--reportFilePath",
                description: "Path to the output report file.",
                getDefaultValue: () => "report.xml");

            var settingsFilePathOption = new Option<string>
                (name: "--settingsFilePath",
                description: "Path to JSON file with settings.",
                getDefaultValue: () => "appsettings.json");

            command.AddOption(metricsFilePathOption);
            command.AddOption(reportFilePathOption);
            command.AddOption(settingsFilePathOption);

            CLHelper.Configure(command, args, (result) =>
            {
                var clGradeArgs = new CLGradeArgments()
                {
                    MetricsFilePath = result.GetValueForOption(metricsFilePathOption),
                    ReportFilePath = result.GetValueForOption(reportFilePathOption),
                    SettingsFilePath = result.GetValueForOption(settingsFilePathOption)
                };

                RunGradeAsync(clGradeArgs).ConfigureAwait(false).GetAwaiter().GetResult();
            });

            return command;
        }

        #endregion Public Methods

        #region Private Methods

        private static async Task<int> RunGradeAsync(CLGradeArgments args)
        {
            var hostBuilder = new HostBuilder()
                .SetupReportWalker()
                .SetupReportGraderFactory()
                .SetupDeltaGrader()
                .SetupGradeProvider()
                .SetupDeltaSeverityProvider()
                .SetupXmlReportWriter(args.ReportFilePath)
                .SetupMetricsReportStripper()
                .ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddConsole();
                    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                });

            var host = hostBuilder.ConfigureAppConfiguration((hostingContext, cfgBuilder) =>
            {
                cfgBuilder.AddJsonFile("appsettings.json", optional: true);

                if (File.Exists(args.SettingsFilePath))
                    cfgBuilder.AddJsonFile(args.SettingsFilePath, optional: true);

                var cfgRoot = cfgBuilder.AddEnvironmentVariables()
                .Build();

                hostBuilder.ConfigureServices(sc =>
                {
                    sc.Configure<GradingThresholds>(cfgRoot.GetSection("GradingThresholds"));
                });
            }).Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            try
            {
                //if (!rootClSettings.ValidateSettings(logger))
                //    return -1;

                XmlCodeMetricsReport? reportModel = null;
                string errorMessage = null;

                if (args.MetricsFilePath is not null && !XmlHelper.TryRestoreFromXml(
                    args.MetricsFilePath,
                    out reportModel,
                    out errorMessage))
                {
                    logger.LogError($"Unable to restore CodeMetricsReport given at path '{args.MetricsFilePath}'. Reason: {errorMessage}");
                    return -1;
                }

                if (reportModel is null)
                    throw new InvalidOperationException($"{nameof(reportModel)} is not expected to be null here.");

                var outputReportFilePath = args.ReportFilePath;

                using var tokenSource = new CancellationTokenSource();
                Console.CancelKeyPress += delegate
                {
                    tokenSource.Cancel();
                };

                var reportWalker = host.Services.GetRequiredService<IReportWalker>();
                var reportVisitorFactory = host.Services.GetRequiredService<IReportVisitorFactory>();

                var reportGrader = reportVisitorFactory.Create<ReportGrader>();

                await reportWalker.WalkTroughAsync(reportModel, reportGrader, tokenSource.Token);

                var reportValidator = reportVisitorFactory.Create<ReportValidator>();

                await reportWalker.WalkTroughAsync(reportModel, reportValidator, tokenSource.Token);

                if (reportValidator.PoorMetricsCount > 0)
                {
                    logger.LogWarning($"Poor metrics: {reportValidator.PoorMetricsCount}");
                }

                if (reportValidator.BadMetricsCount > 0)
                {
                    logger.LogError($"Bad metrics: {reportValidator.BadMetricsCount}");
                    return 1;
                }

                return 0;
            }
            catch (OperationCanceledException)
            {
                logger.LogError("Operation Cancelled.");
                return -1;
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Exception occurred:{Environment.NewLine}{ex}");
                return -2;
            }
            finally
            {
                //NOTE: This is here due to some problems with ConsoleLoggerProcessor.
                //It minimizes issue with missing final messages
                Thread.Sleep(1500);
            }
        }

        #endregion Private Methods
    }
}