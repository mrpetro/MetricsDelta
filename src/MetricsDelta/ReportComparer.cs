using MetricsDelta;
using MetricsDelta.Helpers;
using MetricsDelta.Model;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MetricsDelta
{
    public class ReportComparer
    {
        #region Private Fields

        private readonly IReportVisitor visitor;

        #endregion

        #region Public Constructors

        public ReportComparer(IReportVisitor visitor)
        {
            this.visitor = visitor;
        }

        #endregion

        #region Public Methods

        public async Task CompareAsync(ICodeMetricsReport previous, ICodeMetricsReport current, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(previous);
            ArgumentNullException.ThrowIfNull(current);

            cancellationToken.ThrowIfCancellationRequested();

            visitor.BeginVisitReport();

            var previousTargetsLookup = previous.Targets.ToDictionary(item => item.Name, item => item);
            var currentTargetsLookup = current.Targets.ToDictionary(item => item.Name, item => item);

            foreach (var currentTarget in current.Targets)
            {
                if (!previousTargetsLookup.TryGetValue(currentTarget.Name, out var previousTarget))
                {
                    await ProcessNewTargetAsync(currentTarget, cancellationToken);
                    continue;
                }

                await ProcessExistingTargetAsync(previousTarget, currentTarget, cancellationToken);
            }

            foreach (var previousTarget in previous.Targets)
            {
                if (!currentTargetsLookup.ContainsKey(previousTarget.Name))
                {
                    await ProcessRemovedTargetAsync(previousTarget, cancellationToken);
                }
            }

            visitor.EndVisitReport();

            await Task.CompletedTask;
        }

        #endregion

        #region Private Methods

        private async Task ProcessNewAssemblyAsync(IAssembly assembly, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrEmpty(assembly.Name))
                return;

            visitor.BeginVisitAssembly(assembly.Name, DeltaState.New);

            foreach (var currentMetric in assembly.Metrics)
            {
                visitor.VisitMetric(DeltaState.New, currentMetric.Name, currentMetric.Value, 0);
            }

            visitor.EndVisitAssembly(assembly.Name, DeltaState.New);

            await Task.CompletedTask;
        }

        private async Task ProcessNewTargetAsync(ITarget target, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrEmpty(target.Name))
                return;

            visitor.BeginVisitTarget(target.Name, DeltaState.New);

            if(target.Assembly is not null)
                await ProcessNewAssemblyAsync(target.Assembly, cancellationToken);

            visitor.EndVisitTarget(target.Name, DeltaState.New);

            await Task.CompletedTask;
        }

        private async Task ProcessExistingAssemblyAsync(IAssembly previousAssembly, IAssembly currentAssembly, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            visitor.BeginVisitAssembly(currentAssembly.Name, DeltaState.Existing);

            foreach (var currentMetric in currentAssembly.Metrics)
            {
                var previousMetric = previousAssembly.Metrics.FirstOrDefault(metric => metric.Name == currentMetric.Name);

                //Check if current metric is NEW
                if (previousMetric is null)
                {
                    visitor.VisitMetric(DeltaState.New, currentMetric.Name, currentMetric.Value, 0);
                    continue;
                }

                visitor.VisitMetric(DeltaState.Existing, currentMetric.Name, currentMetric.Value, currentMetric.Value - previousMetric.Value);
            }

            foreach (var previousMetric in previousAssembly.Metrics)
            {
                var currentMetric = currentAssembly.Metrics.FirstOrDefault(metric => metric.Name == previousMetric.Name);

                if (currentMetric is null)
                {
                    visitor.VisitMetric(DeltaState.Removed, previousMetric.Name, previousMetric.Value, 0);
                }
            }

            visitor.EndVisitAssembly(currentAssembly.Name, DeltaState.Existing);

            await Task.CompletedTask;
        }

        private async Task ProcessExistingTargetAsync(ITarget previousTarget, ITarget currentTarget, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            visitor.BeginVisitTarget(currentTarget.Name, DeltaState.Existing);

            if(previousTarget.Assembly is not null && currentTarget.Assembly is not null)
                await ProcessExistingAssemblyAsync(previousTarget.Assembly, currentTarget.Assembly, cancellationToken);

            visitor.EndVisitTarget(currentTarget.Name, DeltaState.Existing);

            await Task.CompletedTask;
        }

        private async Task ProcessRemovedTargetAsync(ITarget target, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            visitor.BeginVisitTarget(target.Name, DeltaState.Removed);

            if(target.Assembly is not null)
                await ProcessRemovedAssemblyAsync(target.Assembly, cancellationToken);

            visitor.EndVisitTarget(target.Name, DeltaState.Removed);

            await Task.CompletedTask;
        }

        private async Task ProcessRemovedAssemblyAsync(IAssembly assembly, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            visitor.BeginVisitAssembly(assembly.Name, DeltaState.Removed);

            visitor.EndVisitAssembly(assembly.Name, DeltaState.Removed);

            await Task.CompletedTask;
        }

        #endregion
    }
}