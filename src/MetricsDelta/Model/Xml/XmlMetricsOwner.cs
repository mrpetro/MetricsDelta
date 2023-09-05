using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MetricsDelta.Model.Xml
{
    public abstract class XmlMetricsOwner : IMetricsOwner
    {
        [XmlArray("Metrics")]
        [XmlArrayItem("Metric", typeof(XmlMetric))]
        public XmlMetric[]? XmlMetrics { get; set; }

        [XmlIgnore]
        public IEnumerable<IMetric> Metrics => XmlMetrics is null ? Enumerable.Empty<IMetric>() : XmlMetrics;
    }
}
