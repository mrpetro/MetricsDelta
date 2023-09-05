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
        #region Public Properties

        [XmlAttribute("Name")]
        public string? Name { get; set; }

        [XmlAttribute("Value")]
        public int Value { get; set; }

        #endregion Public Properties

        public override string ToString()
        {
            return $"Metric [{Name} = {Value}]";
        }
    }
}
