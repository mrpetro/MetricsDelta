using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;

namespace MetricsDelta.Extensions
{
    /// <summary>
    /// Various extension methods for IHostBuilder.
    /// </summary>
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupMetricsReportGrader(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) => services.AddReportGrader());
        }

        public static void SetupMetricsReportStripper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) => services.AddMetricsReportStripper());
        }

        public static void SetupGradeProvider(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) => services.AddGradeProvider());
        }
        public static void SetupXmlReportWriter(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) => services.AddXmlReportWriter());
        }

        public static void SetupCommandLine(this IHostBuilder hostBuilder, string[] args)
        {
            hostBuilder.ConfigureServices((sc) =>
            {
                var previousMetricsFilePath = new Option<string>
                    (name: "--previousMetricsFilePath",
                    description: "Path to the previous code metrics report file.",
                    getDefaultValue: () => "previous.xml");

                var currentMetricsFilePath = new Option<string>
                    (name: "--currentMetricsFilePath",
                    description: "Path to the current code metrics report file.",
                    getDefaultValue: () => "current.xml");

                var failOnDeltaDegradation = new Option<bool>
                    (name: "--failOnDeltaDegradation", 
                    description: "Program will return 1 if there is at least one delta degradation.",
                    getDefaultValue: () => true);

                var failOnFatalMetricGrade = new Option<bool>
                    (name: "--failOnFatalGrade",
                    description: "Program will return 1 if there is at least one failed metric grade.",
                    getDefaultValue: () => true);

                var reportFilePath = new Option<string>
                    (name: "--reportFilePath",
                    description: "Path to the report file.",
                    getDefaultValue: () => "report.xml");

                var rootCommand = new RootCommand
                {
                    previousMetricsFilePath,
                    currentMetricsFilePath,
                    reportFilePath,
                    failOnDeltaDegradation,
                    failOnFatalMetricGrade
                };

                Configure(rootCommand, args, (result) =>
                {
                    sc.Configure<RunSettings>(settings =>
                    {
                        settings.PreviousMetricsFilePath = result.GetValueForOption(previousMetricsFilePath);
                        settings.CurrentMetricsFilePath = result.GetValueForOption(currentMetricsFilePath);
                        settings.ReportFilePath = result.GetValueForOption(reportFilePath);
                        settings.FailOnDeltaDegradation = result.GetValueForOption(failOnDeltaDegradation);
                        settings.FailOnFatalMetricGrade = result.GetValueForOption(failOnFatalMetricGrade);
                    });

                });
            });
        }

        #endregion Public Methods

        #region Private Methods

        private static void Configure(
            RootCommand rootCommand,
            string[] args,
            Action<ParseResult> resultProvider)
        {
            rootCommand.SetHandler((handle) =>
            {
                resultProvider.Invoke(handle.ParseResult);
            });

            rootCommand.Invoke(args);
        }

        #endregion Private Methods
    }
}
