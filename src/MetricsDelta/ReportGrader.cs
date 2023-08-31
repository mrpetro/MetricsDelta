using Microsoft.Extensions.Logging;

namespace MetricsDelta
{
    public class ReportGrader : IReportVisitor
    {
        #region Private Fields

        private readonly IReportWriter reportWriter;
        private readonly IGradeProvider gradeProvider;
        private readonly ILogger<ReportGrader> logger;

        #endregion Private Fields

        #region Public Constructors

        public ReportGrader(
            IReportWriter reportBuilder,
            IGradeProvider gradeProvider,
            ILogger<ReportGrader> logger)
        {
            this.reportWriter = reportBuilder;
            this.gradeProvider = gradeProvider;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool AnyDeltaDegradations { get; private set; }
        public bool AnyBadMetricGrades { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void VisitMetric(DeltaState deltaState, string metricName, int value, int delta)
        {
            var valueGrade = gradeProvider.GetValueGrade(metricName, value);
            var deltaSeverity = gradeProvider.GetDeltaSeverity(metricName, delta);

            reportWriter.WriteMetric(metricName, value, delta, valueGrade, deltaState, deltaSeverity);

            switch (valueGrade)
            {
                case MetricGrade.Good:
                    logger.LogTrace($"Good metric '{metricName}' = {value}");
                    break;

                case MetricGrade.Poor:
                    logger.LogWarning($"Poor metric '{metricName}' = {value}");
                    break;

                case MetricGrade.Bad:

                    AnyBadMetricGrades = true;
                    logger.LogError($"Bad metric '{metricName}' = {value}");
                    break;

                default:
                    throw new InvalidOperationException($"Unknown ValueGrade '{valueGrade}'.");
            }

            var deltaMessage = $"{GetStatePrefix(deltaState)} {metricName}: {value} ({delta}) {GetSeveritySuffix(deltaSeverity)}";

            switch (deltaSeverity)
            {
                case DeltaSeverity.Irrelevant:
                    logger.LogTrace(deltaMessage);
                    break;

                case DeltaSeverity.Improved:
                    logger.LogInformation(deltaMessage);
                    break;

                case DeltaSeverity.Declined:
                    logger.LogError(deltaMessage);

                    AnyDeltaDegradations = true;

                    break;

                default:
                    throw new InvalidOperationException($"Unknown DeltaSeverity '{deltaSeverity}'.");
            }
        }

        public void BeginVisitTarget(string targetName, DeltaState deltaState)
        {
            var message = $"Project {GetStatePrefix(deltaState)} {targetName} started.";
            logger.LogTrace(message);

            reportWriter.BeginWriteTarget(targetName, deltaState);
        }

        public void EndVisitTarget(string targetName, DeltaState deltaState)
        {
            reportWriter.EndWriteTarget(targetName, deltaState);

            var message = $"Project {GetStatePrefix(deltaState)} {targetName} finished.";
            logger.LogTrace(message);
        }

        public void BeginVisitAssembly(string assemblyName, DeltaState deltaState)
        {
            var message = $"Assembly {GetStatePrefix(deltaState)} {assemblyName} started.";
            logger.LogTrace(message);

            reportWriter.BeginWriteAssembly(assemblyName, deltaState);
        }

        public void EndVisitAssembly(string assemblyName, DeltaState deltaState)
        {
            reportWriter.EndWriteAssembly(assemblyName, deltaState);

            var message = $"Assembly {GetStatePrefix(deltaState)} {assemblyName} finished.";
            logger.LogTrace(message);
        }

        public void BeginVisitReport()
        {
            logger.LogTrace("Report started.");

            reportWriter.BeginWriteReport();
        }

        public void EndVisitReport()
        {
            reportWriter.EndWriteReport();

            logger.LogTrace("Report finished.");
        }

        #endregion Public Methods

        #region Private Methods

        private string GetStatePrefix(DeltaState deltaState)
        {
            switch (deltaState)
            {
                case DeltaState.Existing:
                    return string.Empty;

                case DeltaState.New:
                    return "(NEW) ";

                case DeltaState.Removed:
                    return "(REMOVED) ";

                default:
                    throw new InvalidOperationException($"Unknown DeltaState '{deltaState}'.");
            }
        }

        private string GetSeveritySuffix(DeltaSeverity deltaSeverity)
        {
            switch (deltaSeverity)
            {
                case DeltaSeverity.Irrelevant:
                    return string.Empty;

                case DeltaSeverity.Improved:
                    return "(IMPROVED) ";

                case DeltaSeverity.Declined:
                    return "(DECLINED) ";

                default:
                    throw new InvalidOperationException($"Unknown DeltaSeverity '{deltaSeverity}'.");
            }
        }

        #endregion Private Methods
    }
}