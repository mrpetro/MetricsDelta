namespace MetricsDelta
{
    public interface IDeltaSeverityProvider
    {
        #region Public Methods

        DeltaSeverity GetDeltaSeverity(string metricName, int delta);

        #endregion Public Methods
    }
}