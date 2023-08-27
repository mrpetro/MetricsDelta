using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MetricsDelta.Model.Xml
{
    public class XmlMetric : IMetric
    {
        [XmlAttribute("Name")]
        public string? Name { get; set; }

        [XmlAttribute("Value")]
        public int Value { get; set; }
    }
}
