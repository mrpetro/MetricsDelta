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
    public class ReportCompareCommand : Command
    {
        #region Private Constructors

        private ReportCompareCommand(string name, string? description = null) : base(name, description)
        {
        }

        #endregion Private Constructors

        #region Public Methods

        public static ReportCompareCommand Create(string[] args)
        {
            var command = new ReportCompareCommand("compare", "Compare two given code metrics reports");

            var previousMetricsFilePathOption = new Option<string>
                (name: "--previousMetricsFilePath",
                description: "Path to the previous code metrics report file.");

            var currentMetricsFilePathOption = new Option<string>
                (name: "--currentMetricsFilePath",
                description: "Path to the current code metrics report file.");

            var reportFilePathOption = new Option<string>
                (name: "--reportFilePath",
                description: "Path to the report file.",
                getDefaultValue: () => "report.xml");

            var settingsFilePathOption = new Option<string>
                (name: "--settingsFilePath",
                description: "Path to JSON file with settings.",
                getDefaultValue: () => "appsettings.json");

            command.AddOption(previousMetricsFilePathOption);
            command.AddOption(currentMetricsFilePathOption);
            command.AddOption(reportFilePathOption);
            command.AddOption(settingsFilePathOption);

            CLHelper.Configure(command, args, (result) =>
            {
                var clArgs = new CLCompareArgments()
                {
                    PreviousMetricsFilePath = result.GetValueForOption(previousMetricsFilePathOption),
                    CurrentMetricsFilePath = result.GetValueForOption(currentMetricsFilePathOption),
                    ReportFilePath = result.GetValueForOption(reportFilePathOption),
                    SettingsFilePath = result.GetValueForOption(settingsFilePathOption)
                };

                RunCompareAsync(clArgs).ConfigureAwait(false).GetAwaiter().GetResult();
            });

            return command;
        }

        #endregion Public Methods

        #region Private Methods

        private static async Task<int> RunCompareAsync(CLCompareArgments args)
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

                XmlCodeMetricsReport? previousReportModel = null;
                string errorMessage = null;

                if (args.PreviousMetricsFilePath is not null && !XmlHelper.TryRestoreFromXml(
                    args.PreviousMetricsFilePath,
                    out previousReportModel,
                    out errorMessage))
                {
                    logger.LogError($"Unable to restore CodeMetricsReport given at path '{args.PreviousMetricsFilePath}'. Reason: {errorMessage}");
                    return -1;
                }

                if (previousReportModel is null)
                    throw new InvalidOperationException($"{nameof(previousReportModel)} is not expected to be null here.");


                XmlCodeMetricsReport? currentReportModel = null;
                errorMessage = null;

                if (args.CurrentMetricsFilePath is not null && !XmlHelper.TryRestoreFromXml(
                    args.CurrentMetricsFilePath,
                    out currentReportModel,
                    out errorMessage))
                {
                    logger.LogError($"Unable to restore CodeMetricsReport given at path '{args.CurrentMetricsFilePath}'. Reason: {errorMessage}");
                    return -1;
                }

                if (currentReportModel is null)
                    throw new InvalidOperationException($"{nameof(currentReportModel)} is not expected to be null here.");


                var outputReportFilePath = args.ReportFilePath;

                using var tokenSource = new CancellationTokenSource();
                Console.CancelKeyPress += delegate
                {
                    tokenSource.Cancel();
                };

                var deltaVisitor = host.Services.GetRequiredService<IDeltaVisitor>();
                var reportComparer = new ReportComparer(deltaVisitor);
                var reportVisitorFactory = host.Services.GetRequiredService<IReportVisitorFactory>();

                await reportComparer.CompareAsync(previousReportModel, currentReportModel, tokenSource.Token);
                
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