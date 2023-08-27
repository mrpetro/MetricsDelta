using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MetricsDelta.Model.Xml
{
    public class XmlTarget : ITarget
    {
        [XmlAttribute("Name")]
        public string? Name { get; set; }

        [XmlElement("Assembly")]
        public XmlAssembly? XmlAssembly { get; set; }

        [XmlIgnore]
        public IAssembly? Assembly => XmlAssembly;
    }
}
