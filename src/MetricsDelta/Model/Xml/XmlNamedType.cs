using System.Xml.Serialization;

namespace MetricsDelta.Model.Xml
{
    public class XmlNamedType : XmlMetricsOwner, INamedType
    {
        #region Public Properties

        [XmlAttribute("Name")]
        public string? Name { get; set; }

        [XmlArray("Members")]
        [XmlArrayItem("Field", typeof(XmlField))]
        [XmlArrayItem("Method", typeof(XmlMethod))]
        public XmlMember[]? XmlMembers { get; set; }

        [XmlIgnore]
        public IEnumerable<IMember> Members => XmlMembers is null ? Enumerable.Empty<IMember>() : XmlMembers;

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"NamedType [{Name}]";
        }

        #endregion Public Methods
    }
}