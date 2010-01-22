#region Header

// Copyright (c) ShiverCube 2010
//
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
//    * Redistributions of source code must retain the above copyright
//      notice, this list of conditions and the following disclaimer.
//    * Neither the name of the copyright holder nor the names of its
//      contributors may be used to endorse or promote products derived from
//      this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.

#endregion Header

namespace DynamicXml
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Dynamic;
    using System.IO;
    using System.Xml.Linq;

    /// <summary>
    /// Reads an XML document allowing elements to be read dynamically by properties
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface",
        Justification = "This is not a collection")]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "This is not a collection")]
    public class DynamicXmlReader : DynamicObject, IEnumerable
    {
        #region Fields

        private XmlElement _xml;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DynamicXmlReader class
        /// </summary>
        public DynamicXmlReader()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DynamicXmlReader class with the provided XML string
        /// </summary>
        /// <param name="data">The XML string to load</param>
        public DynamicXmlReader(string data)
        {
            Load(data);
        }

        /// <summary>
        /// Initializes a new instance of the DynamicXmlReader class with the provided XML stream
        /// </summary>
        /// <param name="reader">The XML stream to load</param>
        public DynamicXmlReader(TextReader reader)
        {
            Load(reader);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection of XML items
        /// </summary>
        /// <exception cref="InvalidOperationException">If an XML document hasn't been loaded</exception>
        /// <returns>An enumerator that can iterate through the collection of XML items</returns>
        public virtual IEnumerator GetEnumerator()
        {
            if (_xml == null)
            {
                throw new InvalidOperationException("XML document has not been loaded");
            }

            return _xml.GetEnumerator();
        }

        /// <summary>
        /// Loads the provided XML string
        /// </summary>
        /// <param name="data">The XML string to load</param>
        public void Load(string data)
        {
            // TODO: Document exceptions
            _xml = new XmlElement(XElement.Parse(data));
        }

        /// <summary>
        /// Loads the provided XML stream
        /// </summary>
        /// <param name="reader">The XML stream to load</param>
        public void Load(TextReader reader)
        {
            // TODO: Document exceptions
            _xml = new XmlElement(XElement.Parse(reader.ReadToEnd()));
        }

        /// <summary>
        /// Loads the provided XML file
        /// </summary>
        /// <param name="fileName">The filename of the XML file to load</param>
        public void LoadFile(string fileName)
        {
            // TODO: Document exceptions
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(fileName);
                Load(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Gets an XML element property on request
        /// </summary>
        /// <exception cref="InvalidOperationException">If an XML document hasn't been loaded</exception>
        /// <param name="binder">The binder provided by the call site</param>
        /// <param name="result">The result of the get operation</param>
        /// <returns>As null is returned instead of throwing an Exception when an item is not found, this method will
        /// always return true</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_xml == null)
            {
                throw new InvalidOperationException("XML document has not been loaded");
            }

            return _xml.TryGetMember(binder, out result);
        }

        #endregion Public Methods

        #endregion Methods
    }
}