using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_translation.core.Wrappers
{
    public class ValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="field">Property Field.</param>
        /// <param name="message">Valdation Error Message.</param>
        public ValidationError(string field, string message)
        {
            this.Field = field != string.Empty ? field : null;
            this.Message = message;
        }

        /// <summary>
        /// Gets Field.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        /// <summary>
        /// Gets Valdiation Error Message.
        /// </summary>
        public string Message { get; }
    }
}
