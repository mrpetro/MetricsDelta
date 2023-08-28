namespace MetricsDelta.Configuration
{
    public class MetricThresholdsCfg
    {
        #region Public Properties

        public MetricThresholdCfg MaintainabilityIndex { get; } = new MetricThresholdCfg(20, 10);
        public MetricThresholdCfg CyclomaticComplexity { get; } = new MetricThresholdCfg(50, 300);
        public MetricThresholdCfg ClassCoupling { get; } = new MetricThresholdCfg(10, 70);
        public MetricThresholdCfg DepthOfInheritance { get; } = new MetricThresholdCfg(3, 5);
        public MetricThresholdCfg ExecutableLines { get; } = new MetricThresholdCfg(500, 5000);
        public MetricThresholdCfg SourceLines { get; } = new MetricThresholdCfg(1000, 10000);

        #endregion Public Properties
    }
}