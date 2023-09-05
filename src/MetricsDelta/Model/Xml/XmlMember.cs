using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MetricsDelta.Model.Xml
{
    public abstract class XmlMember : XmlMetricsOwner, IMember
    {
        [XmlAttribute("Name")]
        public string? Name { get; set; }
    }
}
