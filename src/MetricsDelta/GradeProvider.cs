using MetricsDelta.Configuration;
using MetricsDelta.Model;
using Microsoft.Extensions.Options;

namespace MetricsDelta
{
    public class GradeProvider : IGradeProvider
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
                    throw new NotImplementedException(metricName);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private MetricGrade GradeMaintainabilityIndex(int value)
            => GradeHigherTheBetter(value, thresholds.MaintainabilityIndex);

        private MetricGrade GradeCyclomaticComplexity(int value)
            => GradeLowerTheBetter(value, thresholds.CyclomaticComplexity);

        private MetricGrade GradeClassCoupling(int value)
            => GradeLowerTheBetter(value, thresholds.ClassCoupling);

        private MetricGrade GradeDepthOfInheritance(int value)
            => GradeLowerTheBetter(value, thresholds.DepthOfInheritance);

        private MetricGrade GradeSourceLines(int value)
            => GradeLowerTheBetter(value, thresholds.SourceLines);

        private MetricGrade GradeExecutableLines(int value)
            => GradeLowerTheBetter(value, thresholds.ExecutableLines);

        private MetricGrade GradeHigherTheBetter(int value, GradingThreshold cfg)
        {
            if (value > cfg.Poor) return MetricGrade.Good;
            else if (value <= cfg.Poor && value > cfg.Bad) return MetricGrade.Poor;
            else return MetricGrade.Bad;
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