using System.Xml.Serialization;

namespace MetricsDelta.Model.Xml
{
    public class XmlMetric : IMetric
    {
        #region Public Properties

        [XmlAttribute("Name")]
        public string? Name { get; set; }

        [XmlAttribute("Value")]
        public int Value { get; set; }

        [XmlAttribute("Grade")]
        public string? Grade { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"Metric [{Name} = {Value}]";
        }

        #endregion Public Methods
    }
}