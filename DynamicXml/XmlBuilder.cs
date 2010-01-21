using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DynamicXml
{
    /// <summary>
    /// Contains common static functions related to generating XML items
    /// </summary>
    public static class XmlBuilder
    {
        private static string[] specialChars = { "&", "<", ">", "\"", "'", "-" };
        private static string[] entities = { "&amp;", "&lt;", "&gt;", "&quot;", "&#39;", "&#45;" };

        /// <summary>
        /// Returns either an XmlElement instance or a string value depending on the provided XElement instance
        /// </summary>
        /// <param name="xelement">The XElement instance to generate an XML item for</param>
        /// <returns>The appropriate XML item type for the given XElement instance</returns>
        internal static object GenerateItem(XElement xelement)
        {
            if (xelement.HasElements)
            {
                return new XmlElement(xelement);
            }
            else
            {
                return Decode(xelement.Value);
            }
        }

        /// <summary>
        /// Encodes special XML characters to appropriate entities
        /// </summary>
        /// <param name="value">The string to encode</param>
        /// <returns>The XML safe value string</returns>
        public static string Encode(string value)
        {
            // Replace entities to temporary markers so that ampersands won't get messed up
            value = Regex.Replace(value, @"&#(\d+);", @"__TEMP_AMPERSANDS__\1;");
            value = Regex.Replace(value, @"&(\w+);", @"__TEMP_AMPERSANDS__\1;");

            for (int i = 0, len = specialChars.Length; i < len; ++i)
            {
                value = value.Replace(specialChars[i], entities[i]);
            }

            // Decode the temp markers back to entities
            value = Regex.Replace(value, @"__TEMP_AMPERSANDS__(\d+);", @"&#\1;");
            value = Regex.Replace(value, @"__TEMP_AMPERSANDS__(\w+);", @"&\1;");

            return value;
        }

        /// <summary>
        /// Decodes entities to special XML characters
        /// </summary>
        /// <param name="value">The string to decode</param>
        /// <returns>The decoded value</returns>
        public static string Decode(string value)
        {
            for (int i = 0, len = specialChars.Length; i < len; ++i)
            {
                value = value.Replace(entities[i], specialChars[i]);
            }

            return value;
        }
    }
}
