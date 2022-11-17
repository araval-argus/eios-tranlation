using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.businesslogic.Features.LabelGroup.ViewModels
{
    public class LabelGroupDetailViewModel
    {
        public int LabelGroupId { get; set; }
        public string GroupName { get; set; }
        public int? FK_ParentLableGroupId { get; set; }
        public LabelGroupViewModel? ParentGroup { get; set; }
        public List<LabelGroupViewModel> ChildGroups { get; set; } = new List<LabelGroupViewModel>();
        public List<LanguageAndLabelDetails> LanguageLabels { get; set; } = new List<LanguageAndLabelDetails>();
        public string BreadCrumb { get; set; }
    }

    public class LanguageAndLabelDetails
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public double? Tolerance { get; set; }
        public ToleranceType ToleranceType { get; set; }
        public bool IsDefault { get; set; }
        public List<LabelDetails> Labels { get; set; } = new List<LabelDetails>();
    }

    public class LabelDetails
    {
        public int LabelId { get; set; }
        public string ResourceId { get; set; }
        public int FK_LabelGroupId { get; set; }
        public int FK_LanguageId { get; set; }
        public string? LabelValue { get; set; }
        public string? MachineTranslation { get; set; }
        public TranslationStatus TranslationStatus { get; set; }
    }


}
