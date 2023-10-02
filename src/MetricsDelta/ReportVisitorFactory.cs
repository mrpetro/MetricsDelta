using Microsoft.Extensions.Logging;
using System.CommandLine.Help;

namespace MetricsDelta
{
    public class ReportVisitorFactory : IReportVisitorFactory
    {
        #region Private Fields

        private readonly IGradeProvider gradeProvider;
        private readonly ILogger<ReportGrader> logger;

        #endregion Private Fields

        #region Public Constructors

        public ReportVisitorFactory(IGradeProvider gradeProvider, ILogger<ReportGrader> logger)
        {
            this.gradeProvider = gradeProvider;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public TVisitor Create<TVisitor>() where TVisitor : IReportVisitor
        {
            if (typeof(TVisitor) == typeof(ReportGrader))
                return (TVisitor)(object)new ReportGrader(gradeProvider, logger);
            else if (typeof(TVisitor) == typeof(ReportValidator))
                return (TVisitor)(object)new ReportValidator(logger);
            else
                throw new InvalidOperationException();
        }


        #endregion Public Methods
    }
}