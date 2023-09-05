﻿using System.Xml.Serialization;

namespace MetricsDelta.Model.Xml
{
    public class XmlAssembly : XmlMetricsOwner, IAssembly
    {
        #region Public Properties

        [XmlAttribute("Name")]
        public string? Name { get; set; }

        [XmlArray("Namespaces")]
        [XmlArrayItem("Namespace", typeof(XmlNamespace))]
        public XmlNamespace[]? XmlNamespaces { get; set; }

        [XmlIgnore]
        public IEnumerable<INamespace> Namespaces => XmlNamespaces is null ? Enumerable.Empty<INamespace>() : XmlNamespaces;

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"Assembly [{Name}]";
        }

        #endregion Public Methods
    }
}