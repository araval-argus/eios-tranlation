using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eios_translation.core.Common;

namespace eios_translation.businesslogic.Features.Label.ViewModels
{
    public class LabelViewModel
    {
        public int LabelId { get; set; }
        public string ResourceId { get; set; }
        public int FK_LabelGroupId { get; set; }
        public int FK_LanguageId { get; set; }
        public string LabelValue { get; set; }
        public LabelType LabelType { get; set; }
        public string LabelDescription { get; set; }
        public string LabelSnapshotPath { get; set; }
        public string MachineTranslation { get; set; }
        public string Scope { get; set; }
        public TranslationStatus TranslationStatus { get; set; }
        public int Version { get; set; }
        public bool IsActive { get; set; }
        public int? FK_PrevVersionLabelId { get; set; }
       
    }
}
