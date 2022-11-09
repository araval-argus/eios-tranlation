

namespace eios_translation.core.Wrappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http.ModelBinding;

    /// <summary>
    /// A class representing  <see cref="ApiError"/>.
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiError"/> class.
        /// </summary>
        public ApiError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiError"/> class.
        /// </summary>
        /// <param name="message">message</param>
        public ApiError(string message)
        {
            this.ExceptionMessage = message;
            this.IsError = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiError"/> class.
        /// </summary>
        /// <param name="modelState">model state</param>
        public ApiError(ModelStateDictionary modelState)
        {
            this.IsError = true;

            if (modelState != null && modelState.Any(m => m.Value.Errors.Count > 0))
            {
                this.ExceptionMessage = "Please correct the specified validation errors and try again.";

                this.ValidationErrors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
            }
        }

        /// <summary>
        /// Gets or sets error details.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets error exception message.
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is error or not.
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// Gets or sets list of validation errors.
        /// </summary>
        public IEnumerable<ValidationError> ValidationErrors { get; set; } = new List<ValidationError>();
    }
}
