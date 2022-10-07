using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_translation.businesslogic.Features.Label.ViewModels
{
    public class LanguageViewModel
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public float ToleranceRelative { get; set; }
        public int ToleranceAbsolute { get; set; }
    }
}
