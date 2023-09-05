namespace MetricsDelta.Model.Xml
{
    public class XmlMethod : XmlMember, IMethod
    {
        #region Public Methods

        public override string ToString()
        {
            return $"Method [{Name}]";
        }

        #endregion Public Methods
    }
}