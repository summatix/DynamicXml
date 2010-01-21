using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
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
        /// Provides the implementation of converting the DynamicObject to another type
        /// </summary>
        /// <param name="binder">The binder provided by the call site</param>
        /// <param name="result">The result of the conversion</param>
        /// <returns>Returns true if the operation is complete, false if the call site should determine
        /// behavior</returns>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            // TODO: Refactor
            if (binder.Type == typeof(List<object>))
            {
                Initialize();
                result = new List<object>(_items);
                return true;
            }

            if (binder.Type == typeof(List<string>))
            {
                Initialize();
                List<string> reference = new List<string>(_items.Count);
                foreach (var item in _items)
                {
                    reference.Add(item.ToString());
                }

                result = reference;
                return true;
            }

            if (binder.Type == typeof(object[]))
            {
                Initialize();
                result = _items.ToArray();
                return true;
            }

            if (binder.Type == typeof(string[]))
            {
                Initialize();
                int count = _items.Count;
                string[] reference = new string[count];
                for (int i = 0; i < count; ++i)
                {
                    reference[i] = _items[i].ToString();
                }

                result = reference;
                return true;
            }

            return base.TryConvert(binder, out result);
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
