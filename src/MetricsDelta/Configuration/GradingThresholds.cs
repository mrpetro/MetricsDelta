namespace MetricsDelta.Configuration
{
    public class GradingThresholds
    {
        #region Public Properties

        public GradingThreshold MaintainabilityIndex { get; set; } = new GradingThreshold(20, 10);
        public GradingThreshold CyclomaticComplexity { get; set; } = new GradingThreshold(50, 300);
        public GradingThreshold ClassCoupling { get; set; } = new GradingThreshold(10, 70);
        public GradingThreshold DepthOfInheritance { get; set; } = new GradingThreshold(3, 5);
        public GradingThreshold ExecutableLines { get; set; } = new GradingThreshold(500, 5000);
        public GradingThreshold SourceLines { get; set; } = new GradingThreshold(1000, 10000);


        #endregion Public Properties
    }
}