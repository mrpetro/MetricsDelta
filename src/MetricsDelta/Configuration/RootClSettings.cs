using Microsoft.Extensions.Logging;

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
        public string? CurrentMetricsFilePath { get; init; }

        /// <summary>
        /// Path to previous (past) code metric report file.
        /// </summary>
        public string? PreviousMetricsFilePath { get; init; }

        /// <summary>
        /// Path to output code metrics report with delta and/or grading information.
        /// </summary>
        public string? ReportFilePath { get; init; }

        /// <summary>
        /// Path to MetricsDelta settings JSON file.
        /// </summary>
        public string? SettingsFilePath { get; init; }

        #endregion Public Properties

        #region Public Methods

        public bool ValidateSettings(ILogger logger)
        {
            if(string.IsNullOrWhiteSpace(PreviousMetricsFilePath))
            {
                logger.LogError($"Parameter --PreviousMetricsFilePath not given or empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(CurrentMetricsFilePath))
            {
                logger.LogError($"Parameter --CurrentMetricsFilePath not given or empty.");
                return false;
            }

            if (!File.Exists(PreviousMetricsFilePath))
            {
                logger.LogError($"Path '{PreviousMetricsFilePath}' given with --PreviousMetricsFilePath doesn't exists.");
                return false;
            }

            if (!File.Exists(CurrentMetricsFilePath))
            {
                logger.LogError($"Path '{CurrentMetricsFilePath}' given with --CurrentMetricsFilePath doesn't exists.");
                return false;
            }

            return true;
        }

        #endregion Public Methods
    }
}