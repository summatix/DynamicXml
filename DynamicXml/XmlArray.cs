#region Header

/*
 * Copyright (c) ShiverCube 2010
 *
 *  All rights reserved.
 *
 *  Redistribution and use in source and binary forms, with or without
 *  modification, are permitted provided that the following conditions are
 *  met:
 *
 *      * Redistributions of source code must retain the above copyright
 *        notice, this list of conditions and the following disclaimer.
 *      * Neither the name of the copyright holder nor the names of its
 *        contributors may be used to endorse or promote products derived from
 *        this software without specific prior written permission.
 *
 *  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 *  AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 *  IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 *  ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 *  LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 *  CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 *  SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 *  INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 *  CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 *  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
 *  THE POSSIBILITY OF SUCH DAMAGE.
 */

#endregion Header

namespace DynamicXml
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Xml.Linq;

    /// <summary>
    /// Holds a collection of XML related items
    /// </summary>
    internal class XmlArray : XmlItem, IXArray
    {
        #region Fields

        private IEnumerable<XElement> _elements;
        private List<object> _items;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the XmlArray class with the given collection of XElement objects
        /// </summary>
        /// <param name="elements">The XElement objects to initialize this instance with</param>
        public XmlArray(IEnumerable<XElement> elements)
        {
            _elements = elements;
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

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

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Gets the items this instance contains. This method is only ever called once when required
        /// </summary>
        /// <returns>The items this instance contains identified by node names</returns>
        protected override Dictionary<string, object> GetItems()
        {
            return new Dictionary<string, object>();
        }

        #endregion Protected Methods

        #region Private Methods

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

        #endregion Private Methods

        #endregion Methods
    }
}