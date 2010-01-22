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
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    /// <summary>
    /// Contains common static functions related to generating XML items
    /// </summary>
    public static class XmlBuilder
    {
        #region Fields

        private static readonly string[] Entities = { "&amp;", "&lt;", "&gt;", "&quot;", "&#39;", "&#45;" };
        private static readonly int EntitiesLength = Entities.Length;
        private static readonly string[] SpecialChars = { "&", "<", ">", "\"", "'", "-" };
        private static readonly int SpecialCharsLength = SpecialChars.Length;

        #endregion Fields

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// Decodes entities to special XML characters
        /// </summary>
        /// <param name="value">The string to decode</param>
        /// <returns>The decoded value</returns>
        public static string Decode(string value)
        {
            for (int i = 0; i < EntitiesLength; ++i)
            {
                value = value.Replace(Entities[i], SpecialChars[i]);
            }

            return value;
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

            for (int i = 0; i < SpecialCharsLength; ++i)
            {
                value = value.Replace(SpecialChars[i], Entities[i]);
            }

            // Decode the temp markers back to entities
            value = Regex.Replace(value, @"__TEMP_AMPERSANDS__(\d+);", @"&#\1;");
            value = Regex.Replace(value, @"__TEMP_AMPERSANDS__(\w+);", @"&\1;");

            return value;
        }

        #endregion Public Static Methods

        #region Internal Static Methods

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

            return Decode(xelement.Value);
        }

        #endregion Internal Static Methods

        #endregion Methods
    }
}