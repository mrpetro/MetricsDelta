namespace MetricsDelta.Model.Xml
{
    public class XmlField : XmlMember, IField
    {
        #region Public Methods

        public override string ToString()
        {
            return $"Field [{Name}]";
        }

        #endregion Public Methods
    }
}