using System.Xml.Linq;

namespace DynamicXml
{
    /// <summary>
    /// Contains common static functions related to generating XML items
    /// </summary>
    internal static class XmlBuilder
    {
        /// <summary>
        /// Returns either an XmlElement instance or a string value depending on the provided XElement instance
        /// </summary>
        /// <param name="xelement">The XElement instance to generate an XML item for</param>
        /// <returns>The appropriate XML item type for the given XElement instance</returns>
        public static object GenerateItem(XElement xelement)
        {
            if (xelement.HasElements)
            {
                return new XmlElement(xelement);
            }
            else
            {
                return xelement.Value;
            }
        }
    }
}
