using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.core.Constants
{
    /// <summary>
    /// Defines the application CommonSettings.
    /// </summary>
    public static class CommonSettings
    {
        /// <summary>
        /// Gets or sets AppSettings.
        /// </summary>
        public static ApplicationSettings AppSettings { get; set; }

        public static AzureTranslationSettings AzureTranslationSettings { get; set; }

        public static GoogleTranslateSettings GoogleTranslateSettings { get; set; }
    }
}
