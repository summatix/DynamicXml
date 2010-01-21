using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace DynamicXml
{
    /// <summary>
    /// Represents an XML item which is able to contain inner XML items
    /// </summary>
    internal abstract class XmlItem : DynamicObject, IXItem
    {
        private Dictionary<string, object> _items = null;

        /// <summary>
        /// Gets an XML element property on request
        /// </summary>
        /// <param name="binder">The binder provided by the call site</param>
        /// <param name="result">The result of the get operation</param>
        /// <returns>true</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            Initialize();
            _items.TryGetValue(binder.Name, out result);
            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection of XML items
        /// </summary>
        /// <returns>An enumerator that can iterate through the collection of XML items</returns>
        public virtual IEnumerator GetEnumerator()
        {
            Initialize();
            return _items.GetEnumerator();
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
            if (binder.Type == typeof(Dictionary<string, object>))
            {
                result = new Dictionary<string, object>(_items);
                return true;
            }

            return base.TryConvert(binder, out result);
        }

        /// <summary>
        /// Gets the items this instance contains. This method is only ever called once when required
        /// </summary>
        /// <returns>The items this instance contains identified by node names</returns>
        protected abstract Dictionary<string, object> GetItems();

        /// <summary>
        /// Initializes the items array if not already initialized
        /// </summary>
        private void Initialize()
        {
            if (_items == null)
            {
                _items = GetItems();
                if (_items == null)
                {
                    throw new InvalidOperationException("GetItems must not return null");
                }
            }
        }
    }
}
