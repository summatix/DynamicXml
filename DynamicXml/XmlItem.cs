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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;

    /// <summary>
    /// Represents an XML item which is able to contain inner XML items
    /// </summary>
    internal abstract class XmlItem : DynamicObject, IXItem
    {
        #region Fields

        private Dictionary<string, object> _items;

        #endregion Fields

        #region Methods

        #region Public Methods

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
        /// Gets an XML element property on request
        /// </summary>
        /// <param name="binder">The binder provided by the call site</param>
        /// <param name="result">The result of the get operation</param>
        /// <returns>As null is returned instead of throwing an Exception when an item is not found, this method will
        /// always return true</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            Initialize();
            _items.TryGetValue(binder.Name, out result);
            return true;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Gets the items this instance contains. This method is only ever called once when required
        /// </summary>
        /// <returns>The items this instance contains identified by node names</returns>
        protected abstract Dictionary<string, object> GetItems();

        #endregion Protected Methods

        #region Private Methods

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

        #endregion Private Methods

        #endregion Methods
    }
}