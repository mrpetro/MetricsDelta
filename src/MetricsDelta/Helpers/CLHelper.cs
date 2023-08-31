using MetricsDelta.Configuration;
using System;
using System.Collections.Generic;
using System.CommandLine.Parsing;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsDelta.Helpers
{
    public static class CLHelper
    {
        #region Public Methods

        public static RootClSettings SetupCommandLine(string[] args)
        {
            var previousMetricsFilePath = new Option<string>
                (name: "--previousMetricsFilePath",
                description: "Path to the previous code metrics report file.");

            var currentMetricsFilePath = new Option<string>
                (name: "--currentMetricsFilePath",
                description: "Path to the current code metrics report file.");

            var reportFilePath = new Option<string>
                (name: "--reportFilePath",
                description: "Path to the report file.",
                getDefaultValue: () => "report.xml");

            var settingsFilePath = new Option<string>
                (name: "--settingsFilePath",
                description: "Path to JSON file with settings.",
                getDefaultValue: () => "appsettings.json");

            var rootCommand = new RootCommand
            {
                previousMetricsFilePath,
                currentMetricsFilePath,
                reportFilePath,
                settingsFilePath
            };

            var cfg = new RootClSettings();

            Configure(rootCommand, args, (result) =>
            {
                cfg = new RootClSettings()
                {
                    PreviousMetricsFilePath = result.GetValueForOption(previousMetricsFilePath),
                    CurrentMetricsFilePath = result.GetValueForOption(currentMetricsFilePath),
                    ReportFilePath = result.GetValueForOption(reportFilePath),
                    SettingsFilePath = result.GetValueForOption(settingsFilePath)
                };
            });

            if (cfg is null)
                throw new InvalidOperationException("RootCommand handler not called.");

            return cfg;
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
