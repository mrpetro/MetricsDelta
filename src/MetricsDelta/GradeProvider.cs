using MetricsDelta.Configuration;
using MetricsDelta.Model;
using Microsoft.Extensions.Options;

namespace MetricsDelta
{
    internal class GradeProvider : IGradeProvider
    {
        #region Private Fields

        private readonly GradingThresholds thresholds;

        #endregion Private Fields

        #region Public Constructors

        public GradeProvider(IOptions<GradingThresholds> options)
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

        public MetricGrade GetValueGrade(string metricName, int value)
        {
            switch (metricName)
            {
                case MetricDefinitions.MaintainabilityIndex:
                    return GradeMaintainabilityIndex(value);

                case MetricDefinitions.SourceLines:
                    return GradeSourceLines(value);

                case MetricDefinitions.ClassCoupling:
                    return GradeClassCoupling(value);

                case MetricDefinitions.DepthOfInheritance:
                    return GradeDepthOfInheritance(value);

                case MetricDefinitions.CyclomaticComplexity:
                    return GradeCyclomaticComplexity(value);

                case MetricDefinitions.ExecutableLines:
                    return GradeExecutableLines(value);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion Public Methods

        #region Private Methods

        private DeltaSeverity GradeMaintainabilityIndexDelta(int delta)
        {
            if (delta == 0) return DeltaSeverity.Irrelevant;
            return delta > 0 ? DeltaSeverity.Improved : DeltaSeverity.Declined;
        }

        private DeltaSeverity GradeCyclomaticComplexityDelta(int delta)
        {
            if (delta == 0) return DeltaSeverity.Irrelevant;
            return delta < 0 ? DeltaSeverity.Improved : DeltaSeverity.Declined;
        }

        private DeltaSeverity GradeClassCouplingDelta(int delta)
        {
            if (delta == 0) return DeltaSeverity.Irrelevant;
            return delta < 0 ? DeltaSeverity.Improved : DeltaSeverity.Declined;
        }

        private DeltaSeverity GradeDepthOfInheritanceDelta(int delta)
        {
            if (delta == 0) return DeltaSeverity.Irrelevant;
            return delta < 0 ? DeltaSeverity.Improved : DeltaSeverity.Declined;
        }

        private DeltaSeverity GradeSourceLinesDelta(int delta)
        {
            if (delta == 0) return DeltaSeverity.Irrelevant;
            return delta < 0 ? DeltaSeverity.Improved : DeltaSeverity.Declined;
        }

        private DeltaSeverity GradeExecutableLinesDelta(int delta)
        {
            if (delta == 0) return DeltaSeverity.Irrelevant;
            return delta < 0 ? DeltaSeverity.Improved : DeltaSeverity.Declined;
        }

        private MetricGrade GradeMaintainabilityIndex(int value)
            => GradeHigherTheBetter(value, thresholds.MaintainabilityIndex);

        private MetricGrade GradeCyclomaticComplexity(int value)
            => GradeLowerTheBetter(value, thresholds.CyclomaticComplexity);

        private MetricGrade GradeClassCoupling(int value)
            => GradeLowerTheBetter(value, thresholds.ClassCoupling);

        private MetricGrade GradeDepthOfInheritance(int value)
            => GradeLowerTheBetter(value, thresholds.DepthOfInheritance);

        private MetricGrade GradeSourceLines(int value)
        {
            return MetricGrade.Good;
        }

        private MetricGrade GradeExecutableLines(int value)
        {
            return MetricGrade.Good;
        }

        private MetricGrade GradeHigherTheBetter(int value, GradingThreshold cfg)
        {
            if (value < cfg.Poor) return MetricGrade.Bad;
            else if (value >= cfg.Poor && value < cfg.Bad) return MetricGrade.Poor;
            else return MetricGrade.Good;
        }

        private MetricGrade GradeLowerTheBetter(int value, GradingThreshold cfg)
        {
            if (value < cfg.Poor) return MetricGrade.Good;
            else if (value >= cfg.Poor && value < cfg.Bad) return MetricGrade.Poor;
            else return MetricGrade.Bad;
        }

        #endregion Private Methods
    }
}