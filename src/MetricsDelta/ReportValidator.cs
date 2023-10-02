using MetricsDelta.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsDelta
{
    internal class ReportValidator : IReportVisitor
    {
        #region Private Fields

        private readonly ILogger<ReportGrader> logger;

        #endregion Private Fields

        #region Public Constructors

        public ReportValidator(
            ILogger<ReportGrader> logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Properties

        public int PoorMetricsCount { get; private set; }
        public int BadMetricsCount { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void VisitMetric(IMetric metric)
        {
            switch (metric.Grade)
            {
                case "Good":
                    logger.LogTrace($"Good metric '{metric.Name}' = {metric.Value}");
                    break;

                case "Poor":
                    PoorMetricsCount++;
                    logger.LogWarning($"Poor metric '{metric.Name}' = {metric.Value}");
                    break;
                case "Bad":
                    BadMetricsCount++;
                    logger.LogError($"Bad metric '{metric.Name}' = {metric.Value}");
                    break;

                default:
                    throw new InvalidOperationException($"Unknown ValueGrade '{metric.Grade}'.");
            }
        }

        public void BeginVisitTarget(ITarget target)
        {
            var message = $"Validating project '{target.Name}' started.";
            logger.LogTrace(message);
        }

        public void EndVisitTarget(ITarget target)
        {
            var message = $"Validating project '{target.Name}' finished.";
            logger.LogTrace(message);
        }

        public void BeginVisitAssembly(IAssembly assembly)
        {
            var message = $"Validating assembly '{assembly.Name}' started.";
            logger.LogTrace(message);
        }

        public void EndVisitAssembly(IAssembly assembly)
        {
            var message = $"Validating assembly '{assembly.Name}' finished.";
            logger.LogTrace(message);
        }

        public void BeginVisitReport(ICodeMetricsReport report)
        {
            logger.LogTrace("Validating report started.");
        }

        public void EndVisitReport(ICodeMetricsReport report)
        {
            logger.LogTrace("Validating report finished.");
        }

        #endregion Public Methods
    }
}
