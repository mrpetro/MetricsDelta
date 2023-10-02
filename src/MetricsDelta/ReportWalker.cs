using MetricsDelta.Model;

namespace MetricsDelta
{
    public class ReportWalker : IReportWalker
    {
        #region Public Constructors

        public ReportWalker()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task WalkTroughAsync(ICodeMetricsReport report, IReportVisitor visitor, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(report);

            cancellationToken.ThrowIfCancellationRequested();

            visitor.BeginVisitReport(report);

            foreach (var target in report.Targets)
            {
                await WalktroughTargetAsync(target, visitor, cancellationToken);
            }

            visitor.EndVisitReport(report);

            await Task.CompletedTask;
        }

        #endregion Public Methods

        #region Private Methods

        private async Task WalktroughTargetAsync(ITarget target, IReportVisitor visitor, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            visitor.BeginVisitTarget(target);

            if (target.Assembly is not null)
            {
                await WalkTroughAssemblyAsync(target.Assembly, visitor, cancellationToken);
            }

            visitor.EndVisitTarget(target);

            await Task.CompletedTask;
        }

        private void WalktroughMetrics(IEnumerable<IMetric> metrics, IReportVisitor visitor)
        {
            foreach (var metric in metrics)
            {
                visitor.VisitMetric(metric);
            }
        }

        private async Task WalkTroughAssemblyAsync(IAssembly assembly, IReportVisitor visitor, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            visitor.BeginVisitAssembly(assembly);

            WalktroughMetrics(assembly.Metrics, visitor);

            visitor.EndVisitAssembly(assembly);

            await Task.CompletedTask;
        }

        #endregion Private Methods
    }
}