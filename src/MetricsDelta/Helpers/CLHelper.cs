using System.CommandLine;
using System.CommandLine.Parsing;

namespace MetricsDelta.Helpers
{
    public static class CLHelper
    {
        #region Public Methods

        public static Command SetupGradeReportCommand(string[] args, Func<CLGradeArgments, Task<int>> func)
        {
            var command = new Command("grade", "Grade given code metrics report");

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

            Configure(command, args, (result) =>
            {
                var clGradeArgs = new CLGradeArgments()
                {
                    MetricsFilePath = result.GetValueForOption(metricsFilePathOption),
                    ReportFilePath = result.GetValueForOption(reportFilePathOption),
                    SettingsFilePath = result.GetValueForOption(settingsFilePathOption)
                };

                var res = func.Invoke(clGradeArgs);
            });

            return command;
        }

        public static Command SetupDifferenceReportCommand(string[] args)
        {
            var command = new Command("compare", "Compare two given code metrics reports");

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

            command.AddOption(previousMetricsFilePath);
            command.AddOption(currentMetricsFilePath);
            command.AddOption(reportFilePath);
            command.AddOption(settingsFilePath);

            return command;
        }

        public static void Configure(
            Command command,
            string[] args,
            Action<ParseResult> resultProvider)
        {
            command.SetHandler((handle) =>
            {
                resultProvider.Invoke(handle.ParseResult);
            });
        }

        #endregion Public Methods
    }

    public class CLGradeArgments
    {
        #region Public Properties

        public string MetricsFilePath { get; init; }
        public string ReportFilePath { get; init; }
        public string SettingsFilePath { get; init; }

        #endregion Public Properties
    }

    public class CLCompareArgments
    {
        #region Public Properties

        public string PreviousMetricsFilePath { get; init; }
        public string CurrentMetricsFilePath { get; init; }
        public string ReportFilePath { get; init; }
        public string SettingsFilePath { get; init; }

        #endregion Public Properties
    }
}