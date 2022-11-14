using eios_translation.core.Common;
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
        public float Tolerance { get; set; }
        public ToleranceType ToleranceType { get; set; }
        public string? Description { get; set; }
        public bool IsDefault { get; set; }
    }
}
