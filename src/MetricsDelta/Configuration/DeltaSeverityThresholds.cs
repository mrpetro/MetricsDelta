namespace MetricsDelta.Configuration
{
    public class DeltaSeverityThresholds
    {
        #region Public Properties

        public Threshold MaintainabilityIndex { get; set; } = new Threshold(-3, -6);
        public Threshold CyclomaticComplexity { get; set; } = new Threshold(5, 30);
        public Threshold ClassCoupling { get; set; } = new Threshold(6, 14);
        public Threshold DepthOfInheritance { get; set; } = new Threshold(3, 5);
        public Threshold ExecutableLines { get; set; } = new Threshold(int.MaxValue / 2, int.MaxValue);
        public Threshold SourceLines { get; set; } = new Threshold(int.MaxValue / 2, int.MaxValue);


        #endregion Public Properties
    }
}