namespace MetricsDelta.Model
{
    public interface IMetricsOwner
    {
        #region Public Properties

        IEnumerable<IMetric> Metrics { get; }

        #endregion Public Properties
    }
}