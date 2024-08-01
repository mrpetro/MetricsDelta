using MetricsDelta.Configuration;
using MetricsDelta.Model;
using Microsoft.Extensions.Options;

namespace MetricsDelta
{
    internal class DeltaSeverityProvider : IDeltaSeverityProvider
    {
        #region Private Fields

        private readonly DeltaSeverityThresholds thresholds;

        #endregion Private Fields

        #region Public Constructors

        public DeltaSeverityProvider(IOptions<DeltaSeverityThresholds> options)
        {
            thresholds = options.Value;
        }

        #endregion Public Constructors

        #region Public Methods

        public DeltaSeverity GetDeltaSeverity(string metricName, int delta)
        {
            switch (metricName)
            {
                case MetricDefinitions.MaintainabilityIndex:
                    return GradeMaintainabilityIndexDelta(delta);

                case MetricDefinitions.SourceLines:
                    return GradeSourceLinesDelta(delta);

                case MetricDefinitions.ClassCoupling:
                    return GradeClassCouplingDelta(delta);

                case MetricDefinitions.DepthOfInheritance:
                    return GradeDepthOfInheritanceDelta(delta);

                case MetricDefinitions.CyclomaticComplexity:
                    return GradeCyclomaticComplexityDelta(delta);

                case MetricDefinitions.ExecutableLines:
                    return GradeExecutableLinesDelta(delta);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion Public Methods

        #region Private Methods

        private DeltaSeverity GradeMaintainabilityIndexDelta(int delta) =>
            GradeHigherTheBetter(delta, thresholds.MaintainabilityIndex);

        private DeltaSeverity GradeCyclomaticComplexityDelta(int delta) =>
            GradeLowerTheBetter(delta, thresholds.CyclomaticComplexity);

        private DeltaSeverity GradeClassCouplingDelta(int delta) =>
            GradeLowerTheBetter(delta, thresholds.ClassCoupling);

        private DeltaSeverity GradeDepthOfInheritanceDelta(int delta) =>
            GradeLowerTheBetter(delta, thresholds.DepthOfInheritance);

        private DeltaSeverity GradeSourceLinesDelta(int delta) =>
            GradeLowerTheBetter(delta, thresholds.SourceLines);

        private DeltaSeverity GradeExecutableLinesDelta(int delta) =>
            GradeLowerTheBetter(delta, thresholds.ExecutableLines);

        private DeltaSeverity GradeLowerTheBetter(int value, Threshold cfg)
        {
            switch (value)
            {
                case int n when (n < 0):
                    return DeltaSeverity.Improved;

                case int n when (n >= 0 && n < cfg.Poor):
                    return DeltaSeverity.Irrelevant;

                case int n when (n >= cfg.Poor && n < cfg.Bad):
                    return DeltaSeverity.Declined;

                case int n when (n >= cfg.Bad):
                    return DeltaSeverity.Degraded;

                default:
                    return DeltaSeverity.Irrelevant;
            }    
        }

        private DeltaSeverity GradeHigherTheBetter(int value, Threshold cfg)
        {
            switch (value)
            {
                case int n when (n > 0):
                    return DeltaSeverity.Improved;

                case int n when (n > cfg.Poor && n <= 0 ):
                    return DeltaSeverity.Irrelevant;

                case int n when (n > cfg.Bad && n <= cfg.Poor):
                    return DeltaSeverity.Declined;

                case int n when (n <= cfg.Bad):
                    return DeltaSeverity.Degraded;

                default:
                    return DeltaSeverity.Irrelevant;
            }
        }

        #endregion Private Methods
    }
}