using MetricsDelta.Configuration;
using MetricsDelta.Model;
using Microsoft.Extensions.Options;
using Moq;

namespace MetricsDelta.Test
{
    public class GradeProviderTests
    {
        #region Private Fields

        private MockRepository mockRepository;

        private Mock<IOptions<GradingThresholds>> mockOptions;

        #endregion Private Fields

        #region Public Constructors

        public GradeProviderTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockOptions = this.mockRepository.Create<IOptions<GradingThresholds>>();
        }

        #endregion Public Constructors

        #region Public Methods

        [Theory]
        [InlineData(MetricDefinitions.MaintainabilityIndex, 0, MetricGrade.Bad)]
        [InlineData(MetricDefinitions.MaintainabilityIndex, 19, MetricGrade.Poor)]
        [InlineData(MetricDefinitions.MaintainabilityIndex, 100, MetricGrade.Good)]
        [InlineData(MetricDefinitions.ClassCoupling, 0, MetricGrade.Good)]
        [InlineData(MetricDefinitions.ClassCoupling, 50, MetricGrade.Poor)]
        [InlineData(MetricDefinitions.ClassCoupling, 100, MetricGrade.Bad)]
        [InlineData(MetricDefinitions.DepthOfInheritance, 0, MetricGrade.Good)]
        [InlineData(MetricDefinitions.DepthOfInheritance, 4, MetricGrade.Poor)]
        [InlineData(MetricDefinitions.DepthOfInheritance, 7, MetricGrade.Bad)]
        [InlineData(MetricDefinitions.CyclomaticComplexity, 0, MetricGrade.Good)]
        [InlineData(MetricDefinitions.CyclomaticComplexity, 80, MetricGrade.Poor)]
        [InlineData(MetricDefinitions.CyclomaticComplexity, 400, MetricGrade.Bad)]
        [InlineData(MetricDefinitions.SourceLines, 0, MetricGrade.Good)]
        [InlineData(MetricDefinitions.SourceLines, 5000, MetricGrade.Poor)]
        [InlineData(MetricDefinitions.SourceLines, 12000, MetricGrade.Bad)]
        [InlineData(MetricDefinitions.ExecutableLines, 0, MetricGrade.Good)]
        [InlineData(MetricDefinitions.ExecutableLines, 2500, MetricGrade.Poor)]
        [InlineData(MetricDefinitions.ExecutableLines, 12000, MetricGrade.Bad)]
        public void GetValueGrade_CheckNonExistingMetricNameWithValue_ExpectCorrectGrade(string metricName, int value, MetricGrade expectedGrade)
        {
            // Arrange
            mockOptions.Setup(item => item.Value).Returns(new GradingThresholds());

            var provider = this.CreateProvider();

            // Act
            var result = provider.GetValueGrade(
                metricName,
                value);

            // Assert
            Assert.True(result == expectedGrade);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void GetValueGrade_CheckNonExistingMetricName_ThrowException()
        {
            // Arrange
            mockOptions.Setup(item => item.Value).Returns(new GradingThresholds());

            var provider = this.CreateProvider();

            // Act
            // Assert
            Assert.Throws<NotImplementedException>(() => provider.GetValueGrade("NonExisting", 0));
            this.mockRepository.VerifyAll();
        }

        #endregion Public Methods

        #region Private Methods

        private GradeProvider CreateProvider()
        {
            return new GradeProvider(
                this.mockOptions.Object);
        }

        #endregion Private Methods
    }
}