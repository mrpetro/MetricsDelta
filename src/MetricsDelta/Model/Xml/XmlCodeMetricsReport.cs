using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MetricsDelta.Model.Xml
{
    [XmlRoot("CodeMetricsReport")]
    [Serializable]
    public class XmlCodeMetricsReport : ICodeMetricsReport
    {
        [XmlAttribute("Version")]
        public string? Version { get; set; }

        [XmlArray("Targets")]
        [XmlArrayItem("Target", typeof(XmlTarget))]
        public XmlTarget[]? XmlTargets { get; set; }

        [XmlIgnore]
        public IEnumerable<ITarget> Targets => XmlTargets is null ? Enumerable.Empty<ITarget>() : XmlTargets;
    }
}
