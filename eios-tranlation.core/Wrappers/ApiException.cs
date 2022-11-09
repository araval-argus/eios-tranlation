

namespace eios_translation.core.Wrappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class representing <see cref="ApiException"/>.
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="statusCode">Http Status Code.</param>
        /// <param name="errors">Errors.</param>
        public ApiException(
           string message,
           int statusCode = 500,
           IEnumerable<ValidationError> errors = null)
           : base(message)
        {
            this.StatusCode = statusCode;
            this.Errors = errors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class.
        /// </summary>
        /// <param name="ex">Exception.</param>
        /// <param name="statusCode">Http Status Code.</param>
        public ApiException(Exception ex, int statusCode = 500)
            : base(ex.Message)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Gets or sets errors.
        /// </summary>
        public IEnumerable<ValidationError> Errors { get; set; }

        /// <summary>
        /// Gets or sets status code.
        /// </summary>
        public int StatusCode { get; set; }
    }
}
