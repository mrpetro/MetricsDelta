namespace MetricsDelta.Configuration
{
    public class GradingThresholds
    {
        #region Public Properties

        public Threshold MaintainabilityIndex { get; set; } = new Threshold(50, 20);
        public Threshold CyclomaticComplexity { get; set; } = new Threshold(50, 300);
        public Threshold ClassCoupling { get; set; } = new Threshold(10, 70);
        public Threshold DepthOfInheritance { get; set; } = new Threshold(12, 15);
        public Threshold ExecutableLines { get; set; } = new Threshold(int.MaxValue / 2, int.MaxValue);
        public Threshold SourceLines { get; set; } = new Threshold(int.MaxValue / 2, int.MaxValue);


        #endregion Public Properties
    }
}