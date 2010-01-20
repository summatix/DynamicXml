using System;
using System.Collections;
using System.Dynamic;
using System.IO;
using System.Xml.Linq;

namespace DynamicXml
{
    /// <summary>
    /// Reads an XML document allowing elements to be read dynamically by properties
    /// </summary>
    public class DynamicXmlReader : DynamicObject, IEnumerable
    {
        private XmlElement _xml = null;

        /// <summary>
        /// Initializes the default instance
        /// </summary>
        public DynamicXmlReader()
        {
        }

        /// <summary>
        /// Initializes the instance with the provided XML string
        /// </summary>
        /// <param name="data">The XML string to load</param>
        public DynamicXmlReader(string data)
        {
            Load(data);
        }

        /// <summary>
        /// Initializes the instance with the provided XML stream
        /// </summary>
        /// <param name="data">The XML stream to load</param>
        public DynamicXmlReader(TextReader reader)
        {
            Load(reader);
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
        /// <param name="filename">The filename of the XML file to load</param>
        public void LoadFile(string filename)
        {
            // TODO: Document exceptions
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(filename);
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
        /// <returns>true</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_xml == null)
            {
                throw new InvalidOperationException("XML document has not been loaded");
            }

            return _xml.TryGetMember(binder, out result);
        }

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
    }
}
