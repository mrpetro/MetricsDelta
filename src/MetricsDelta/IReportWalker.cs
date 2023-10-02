using MetricsDelta.Model;

namespace MetricsDelta
{
    public interface IReportWalker
    {
        #region Public Methods

        Task WalkTroughAsync(ICodeMetricsReport report, IReportVisitor reportVisitor, CancellationToken cancellationToken);

        #endregion Public Methods
    }
}