using Microsoft.Extensions.Configuration;

namespace MetricsDelta.Configuration
{
    /// <summary>
    /// Root command line settings
    /// </summary>
    public class RootClSettings
    {
        #region Public Properties

        /// <summary>
        /// Path to current (present) code metric report file.
        /// </summary>
        public string CurrentMetricsFilePath { get; init; }

        /// <summary>
        /// Path to previous (past) code metric report file.
        /// </summary>
        public string PreviousMetricsFilePath { get; init; }

        /// <summary>
        /// Path to output code metrics report with delta and/or grading information.
        /// </summary>
        public string ReportFilePath { get; init; }

        /// <summary>
        /// Path to MetricsDelta settings JSON file.
        /// </summary>
        public string SettingsFilePath { get; init; }

        #endregion Public Properties
    }
}