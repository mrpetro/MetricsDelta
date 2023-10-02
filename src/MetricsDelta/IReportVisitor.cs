using MetricsDelta.Model;

namespace MetricsDelta
{
    public interface IReportVisitor
    {
        #region Public Methods

        void VisitMetric(IMetric metric);

        void BeginVisitTarget(ITarget target);

        void EndVisitTarget(ITarget target);

        void BeginVisitAssembly(IAssembly assembly);

        void EndVisitAssembly(IAssembly assembly);

        void BeginVisitReport(ICodeMetricsReport report);

        void EndVisitReport(ICodeMetricsReport report);

        #endregion Public Methods
    }
}