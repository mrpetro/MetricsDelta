using MetricsDelta.Model;
using Microsoft.Extensions.Logging;

namespace MetricsDelta
{
    public class ReportGrader : IReportVisitor
    {
        #region Private Fields

        private readonly IGradeProvider gradeProvider;
        private readonly ILogger<ReportGrader> logger;

        private string? currentLevel;

        #endregion Private Fields

        #region Public Constructors

        public ReportGrader(
                    IGradeProvider gradeProvider,
            ILogger<ReportGrader> logger)
        {
            this.gradeProvider = gradeProvider;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public void VisitMetric(IMetric metric)
        {
            if (metric.Name is null)
                return;

            var valueGrade = gradeProvider.GetValueGrade(metric.Name, metric.Value);
            metric.Grade = valueGrade.ToString();
        }

        public void BeginVisitTarget(ITarget target)
        {
            currentLevel = "Target";
            var message = $"Grading project '{target.Name}' started.";
            logger.LogTrace(message);
        }

        public void EndVisitTarget(ITarget target)
        {
            var message = $"Grading project '{target.Name}' finished.";
            logger.LogTrace(message);
            currentLevel = null;
        }

        public void BeginVisitAssembly(IAssembly assembly)
        {
            currentLevel = "Assembly";
            var message = $"Grading assembly '{assembly.Name}' started.";
            logger.LogTrace(message);
        }

        public void EndVisitAssembly(IAssembly assembly)
        {
            var message = $"Grading assembly '{assembly.Name}' finished.";
            logger.LogTrace(message);
            currentLevel = null;
        }

        public void BeginVisitReport(ICodeMetricsReport report)
        {
            currentLevel = "Report";
            logger.LogTrace("Grading report started.");
        }

        public void EndVisitReport(ICodeMetricsReport report)
        {
            logger.LogTrace("Grading report finished.");
            currentLevel = null;
        }

        #endregion Public Methods
    }
}