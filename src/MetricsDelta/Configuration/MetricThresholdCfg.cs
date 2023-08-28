namespace MetricsDelta.Configuration
{
    public class MetricThresholdCfg
    {
        #region Public Constructors

        public MetricThresholdCfg(int poor, int bad)
        {
            Poor = poor;
            Bad = bad;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Bad { get; }
        public int Poor { get; }

        #endregion Public Properties
    }
}