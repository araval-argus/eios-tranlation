using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.businesslogic.Features.Label.ViewModels
{
    public class AzureTranslationResponse
    {
        public List<Translation> Translations { get; set; }

    }

    public class Translation
    {
        public string Text { get; set; }
        public string To { get; set; }
    }

}
