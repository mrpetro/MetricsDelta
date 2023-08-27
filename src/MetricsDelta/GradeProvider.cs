using MetricsDelta.Model;

namespace MetricsDelta
{
    internal class GradeProvider : IGradeProvider
    {
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

        #endregion

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
        {
            if (value < 10) return MetricGrade.Fatal;
            else if (value >= 10 && value < 20) return MetricGrade.Poor;
            else return MetricGrade.Good;
        }

        private MetricGrade GradeCyclomaticComplexity(int value)
        {
            if (value < 10) return MetricGrade.Good;
            else if (value >= 10 && value < 20) return MetricGrade.Poor;
            else return MetricGrade.Fatal;
        }

        private MetricGrade GradeClassCoupling(int value)
        {
            if (value < 50) return MetricGrade.Good;
            else if (value >= 50 && value < 300) return MetricGrade.Poor;
            else return MetricGrade.Fatal;
        }

        private MetricGrade GradeDepthOfInheritance(int value)
        {
            if (value < 3) return MetricGrade.Good;
            else if (value >= 3 && value < 5) return MetricGrade.Poor;
            else return MetricGrade.Fatal;
        }

        private MetricGrade GradeSourceLines(int value)
        {
            return MetricGrade.Good;
        }

        private MetricGrade GradeExecutableLines(int value)
        {
            return MetricGrade.Good;
        }


        #endregion
    }
}