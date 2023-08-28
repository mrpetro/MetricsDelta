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
using System.Runtime.CompilerServices;
using MetricsDelta.Configuration;

namespace MetricsDelta.Extensions
{
    /// <summary>
    /// Various extension methods for IHostBuilder.
    /// </summary>
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static IHostBuilder SetupMetricsReportGrader(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((hostContext, services) => services.AddReportGrader());
        }

        public static IHostBuilder SetupMetricsReportStripper(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((hostContext, services) => services.AddMetricsReportStripper());
        }

        public static IHostBuilder SetupGradeProvider(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((hostContext, services) => services.AddGradeProvider());
        }
        public static IHostBuilder SetupXmlReportWriter(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((hostContext, services) => services.AddXmlReportWriter());
        }

        public static IHostBuilder SetupCommandLine(this IHostBuilder hostBuilder, string[] args)
        {
            return hostBuilder.ConfigureServices((sc) =>
            {
                var previousMetricsFilePath = new Option<string>
                    (name: "--previousMetricsFilePath",
                    description: "Path to the previous code metrics report file.",
                    getDefaultValue: () => "previous.xml");

                var currentMetricsFilePath = new Option<string>
                    (name: "--currentMetricsFilePath",
                    description: "Path to the current code metrics report file.",
                    getDefaultValue: () => "current.xml");

                var reportFilePath = new Option<string>
                    (name: "--reportFilePath",
                    description: "Path to the report file.",
                    getDefaultValue: () => "report.xml");

                var rootCommand = new RootCommand
                {
                    previousMetricsFilePath,
                    currentMetricsFilePath,
                    reportFilePath
                };

                Configure(rootCommand, args, (result) =>
                {
                    sc.Configure<MetricDeltaCfg>(settings =>
                    {
                        settings.PreviousMetricsFilePath = result.GetValueForOption(previousMetricsFilePath);
                        settings.CurrentMetricsFilePath = result.GetValueForOption(currentMetricsFilePath);
                        settings.ReportFilePath = result.GetValueForOption(reportFilePath);
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
