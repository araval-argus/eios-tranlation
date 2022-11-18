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
        public List<LabelWithLanguage> Labels { get; set; } = new List<LabelWithLanguage>();
        public string BreadCrumb { get; set; }
    }

    public class LabelWithLanguage
    {
        public string LanguageName { get; set; }
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        public int FK_LabelGroupId { get; set; }
        public int FK_LanguageId { get; set; }
        public int? FK_BaseLabelId { get; set; }
        public string? LabelValue { get; set; }
        public string? MachineTranslation { get; set; }
        public TranslationStatus TranslationStatus { get; set; }
        public List<LabelWithLanguage> TranslatedLabels { get; set; } = new List<LabelWithLanguage>();

    }
}
