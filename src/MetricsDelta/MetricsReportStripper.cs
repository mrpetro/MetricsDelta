using System.Xml;

namespace MetricsDelta
{
    internal class MetricsReportStripper : IMetricsReportStripper
    {
        public void Strip(string inputFilePath, string outputFilePath)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(inputFilePath);
            ArgumentNullException.ThrowIfNullOrEmpty(outputFilePath);

            using var input = new XmlTextReader(inputFilePath);
            input.WhitespaceHandling = WhitespaceHandling.None;

            var sts = new XmlWriterSettings()
            {
                Indent = true,
            };

            using XmlWriter writer = XmlWriter.Create(outputFilePath, sts);

            while (input.Read())
            {
                switch (input.NodeType)
                {
                    case XmlNodeType.None:
                        break;

                    case XmlNodeType.Element:

                        if (input.Name == "Namespaces")
                        {
                            input.Skip();
                            writer.WriteEndElement();

                            continue;
                        }

                        writer.WriteStartElement(input.Name);

                        for (int i = 0; i < input.AttributeCount; i++)
                        {
                            input.MoveToAttribute(i);

                            writer.WriteAttributeString(input.Name, input.Value);
                        }

                        input.MoveToElement();

                        if (input.IsEmptyElement)
                        {
                            writer.WriteEndElement();
                        }

                        break;

                    case XmlNodeType.Attribute:
                        break;

                    case XmlNodeType.Text:
                        break;

                    case XmlNodeType.CDATA:
                        break;

                    case XmlNodeType.EntityReference:
                        break;

                    case XmlNodeType.Entity:
                        break;

                    case XmlNodeType.ProcessingInstruction:
                        break;

                    case XmlNodeType.Comment:
                        break;

                    case XmlNodeType.Document:
                        break;

                    case XmlNodeType.DocumentType:
                        break;

                    case XmlNodeType.DocumentFragment:
                        break;

                    case XmlNodeType.Notation:
                        break;

                    case XmlNodeType.Whitespace:
                        break;

                    case XmlNodeType.SignificantWhitespace:
                        break;

                    case XmlNodeType.EndElement:
                        writer.WriteEndElement();
                        break;

                    case XmlNodeType.EndEntity:
                        break;

                    case XmlNodeType.XmlDeclaration:
                        writer.WriteStartDocument();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}