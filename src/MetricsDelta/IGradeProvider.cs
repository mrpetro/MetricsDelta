namespace MetricsDelta
{
    public interface IGradeProvider
    {
        #region Public Methods

        DeltaSeverity GetDeltaSeverity(string metricName, int delta);

        MetricGrade GetValueGrade(string metricName, int value);

        #endregion
    }
}