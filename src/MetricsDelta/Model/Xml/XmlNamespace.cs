using System.Xml.Serialization;

namespace MetricsDelta.Model.Xml
{
    public class XmlNamespace : XmlMetricsOwner, INamespace
    {
        #region Public Properties

        [XmlAttribute("Name")]
        public string? Name { get; set; }

        [XmlArray("Types")]
        [XmlArrayItem("NamedType", typeof(XmlNamedType))]
        public XmlNamedType[]? XmlTypes { get; set; }

        [XmlIgnore]
        public IEnumerable<INamedType> Types => XmlTypes is null ? Enumerable.Empty<INamedType>() : XmlTypes;

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"Namespace [{Name}]";
        }

        #endregion Public Methods
    }
}