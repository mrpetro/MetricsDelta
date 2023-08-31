namespace MetricsDelta.Configuration
{
    public class GradingThreshold
    {
        #region Public Constructors

        public GradingThreshold(int poor, int bad)
        {
            Poor = poor;
            Bad = bad;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Bad { get; set; }
        public int Poor { get; set; }

        #endregion Public Properties
    }
}