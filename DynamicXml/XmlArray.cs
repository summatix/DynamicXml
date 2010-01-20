using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DynamicXml
{
    /// <summary>
    /// Holds a collection of XML related items
    /// </summary>
    internal class XmlArray : XmlItem, IXArray
    {
        private IEnumerable<XElement> _elements;
        private List<object> _items = null;

        /// <summary>
        /// Initializes the instance with the given collection of XElement objects
        /// </summary>
        /// <param name="elements">The XElement objects to initialize this instance with</param>
        public XmlArray(IEnumerable<XElement> elements)
        {
            _elements = elements;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection of XML items
        /// </summary>
        /// <returns>An enumerator that can iterate through the collection of XML items</returns>
        public override IEnumerator GetEnumerator()
        {
            Initialize();
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Gets the string representation of this instance
        /// </summary>
        /// <returns>The string representation of this instance</returns>
        public override string ToString()
        {
            return "Array";
        }

        /// <summary>
        /// Gets the items this instance contains. This method is only ever called once when required
        /// </summary>
        /// <returns>The items this instance contains identified by node names</returns>
        protected override Dictionary<string, object> GetItems()
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes the items array if not already initailized
        /// </summary>
        private void Initialize()
        {
            if (_items == null)
            {
                _items = new List<object>();
                foreach (var element in _elements)
                {
                    _items.Add(XmlBuilder.GenerateItem(element));
                }

                _elements = null;
            }
        }
    }
}
