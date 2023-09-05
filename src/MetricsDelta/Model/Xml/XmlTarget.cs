using System.Xml.Serialization;

namespace MetricsDelta.Model.Xml
{
    public class XmlTarget : ITarget
    {
        #region Public Properties

        [XmlAttribute("Name")]
        public string? Name { get; set; }

        [XmlElement("Assembly")]
        public XmlAssembly? XmlAssembly { get; set; }

        [XmlIgnore]
        public IAssembly? Assembly => XmlAssembly;

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"Target [{Name}]";
        }

        #endregion Public Methods
    }
}