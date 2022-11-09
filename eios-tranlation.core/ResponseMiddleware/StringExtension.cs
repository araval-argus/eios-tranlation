

namespace eios_tranlation.core.ResponseMiddleware
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Extension Methods for string.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// An extension method to check if the string is a valid json or not.
        /// </summary>
        /// <param name="text">Input string.</param>
        /// <returns>A boolean indicating if the string is a valid json or not.</returns>
        public static bool IsValidJson(this string text)
        {
            text = text.Trim();

            if ((!text.StartsWith("{") || !text.EndsWith("}")) && (!text.StartsWith("[") || !text.EndsWith("]")))
            {
                return false;
            }

            try
            {
                JToken.Parse(text);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
