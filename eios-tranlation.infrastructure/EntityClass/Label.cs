using eios_translation.core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace eios_translation.infrastructure.EntityClass
{
    public class Label
    {
        [Key]
        public int LabelId { get; protected set; }
        public string ResourceId { get; protected set; } = string.Empty;
        public int FK_LabelGroupId { get; protected set; }
        public int FK_LanguageId { get; protected set; }
        public string LabelValue { get; protected set; } = string.Empty;
        public int LabelType { get; protected set; }
        public string? LabelDescription { get; protected set; }
        public string? LabelSnapshotPath { get; protected set; }
        public string? MachineTranslation { get; protected set; }
        public int TranslationStatus { get; protected set; }
        public string? Scope { get; set; }
        public int Version { get; protected set; }
        public bool IsActive { get; protected set; }
        public int? FK_PrevVersionLabelId { get; protected set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        //[ForeignKey(nameof(FK_LabelGroupId))]
        //public virtual  LabelGroup LabelGroup { get; protected set; }

        //[ForeignKey(nameof(FK_LanguageId))]
        //public virtual Language Language { get; protected set; }

    }
}
