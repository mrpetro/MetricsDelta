namespace MetricsDelta
{
    public interface IGradeProvider
    {
        #region Public Methods

        MetricGrade GetValueGrade(string metricName, int value);

        #endregion
    }
}