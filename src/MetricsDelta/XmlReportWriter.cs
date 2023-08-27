using System.Xml;

namespace MetricsDelta
{
    internal class XmlReportWriter : IReportWriter
    {
        #region Private Fields

        private readonly XmlWriter writer;

        #endregion

        #region Public Constructors

        public XmlReportWriter(XmlWriter writer)
        {
            this.writer = writer;
        }

        #endregion

        #region Public Methods

        public void WriteMetric(string name, int value, int delta, MetricGrade valueGrade, DeltaState deltaState, DeltaSeverity deltaSeverity)
        {
            writer.WriteStartElement("Metric");

            writer.WriteAttributeString("Name", name);

            writer.WriteAttributeString("Value", value.ToString());

            writer.WriteAttributeString("ValueGrade", valueGrade.ToString());

            if (deltaState != DeltaState.Existing)
                writer.WriteAttributeString("Status", deltaState.ToString());

            if (delta != 0)
                writer.WriteAttributeString("Delta", delta.ToString());

            if (deltaSeverity != DeltaSeverity.Irrelevant)
                writer.WriteAttributeString("DeltaGrade", deltaSeverity.ToString());

            writer.WriteEndElement();
        }

        public void BeginWriteTarget(string name, DeltaState deltaState)
        {
            writer.WriteStartElement("Target");
            writer.WriteAttributeString("Name", name);
            writer.WriteAttributeString("Status", deltaState.ToString());
        }

        public void EndWriteTarget(string name, DeltaState deltaState)
        {
            writer.WriteEndElement();
        }

        public void BeginWriteAssembly(string name, DeltaState deltaState)
        {
            writer.WriteStartElement("Assembly");
            writer.WriteAttributeString("Name", name);
            writer.WriteAttributeString("Status", deltaState.ToString());

            writer.WriteStartElement("Metrics");
        }

        public void EndWriteAssembly(string name, DeltaState deltaState)
        {
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        public void BeginWriteReport()
        {
            writer.WriteStartElement("CodeMetricsReport");
            writer.WriteStartElement("Targets");
        }

        public void EndWriteReport()
        {
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Flush();
        }

        #endregion
    }
}