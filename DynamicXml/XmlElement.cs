using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DynamicXml
{
    /// <summary>
    /// Holds a collection of XML related items mapped by node name
    /// </summary>
    internal class XmlElement : XmlItem, IXElement
    {
        private XElement _xml;

        /// <summary>
        /// Initializes the instance with the provided XElement instance
        /// </summary>
        /// <param name="xml">The XElement object to initialize this instance with</param>
        public XmlElement(XElement xml)
        {
            _xml = xml;
        }

        /// <summary>
        /// Gets the items this instance contains. This method is only ever called once when required
        /// </summary>
        /// <returns>The items this instance contains identified by node names</returns>
        protected override Dictionary<string, object> GetItems()
        {
            Dictionary<string, object> items = new Dictionary<string, object>();

            foreach (var group in _xml.Elements().GroupBy(e => e.Name.LocalName))
            {
                if (group.Count() == 1)
                {
                    items[group.Key] = XmlBuilder.GenerateItem(group.First());
                }
                else
                {
                    items[group.Key] = new XmlArray(group.ToArray());
                }
            }

            _xml = null;
            return items;
        }
    }
}
