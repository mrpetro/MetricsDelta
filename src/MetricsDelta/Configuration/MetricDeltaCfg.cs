namespace MetricsDelta.Configuration
{
    public class MetricDeltaCfg
    {
        #region Public Properties

        public string CurrentMetricsFilePath { get; set; }
        public string PreviousMetricsFilePath { get; set; }
        public string ReportFilePath { get; set; }

        public MetricThresholdsCfg Thresholds { get; } = new MetricThresholdsCfg();

        #endregion Public Properties
    }
}