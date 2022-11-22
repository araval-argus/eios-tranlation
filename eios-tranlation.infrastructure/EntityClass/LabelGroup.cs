using eios_tranlation.businesslogic.Features.LabelGroup;
using eios_translation.core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_translation.infrastructure.EntityClass
{
    public class LabelGroup: ITrackable
    {
        public LabelGroup()
        {

        }
        public LabelGroup(string groupname, int? parentgroupid)
        {
            GroupName = groupname;
            FK_ParentLableGroupId = parentgroupid;
        }

        public LabelGroup(string groupname, LabelGroup parentgroup)
        {
            GroupName = groupname;
            ParentGroup = parentgroup;
        }

        [Key]
        public int LabelGroupId { get; protected set; }
        public string GroupName { get; protected set; }
        public int? FK_ParentLableGroupId { get; protected set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        // Referential Properties.

        [ForeignKey(nameof(FK_ParentLableGroupId))]
        public virtual LabelGroup? ParentGroup { get; protected set; }

        //public virtual ICollection<Label> AssociatedLabels { get; protected set; }

        public void UpdateLabelGroup(int labelGroupId, string groupName, int? parentLableGroupId)
        {
            LabelGroupId = labelGroupId;
            GroupName = groupName;
            FK_ParentLableGroupId = parentLableGroupId;
        }

        public void SetGroupName(string groupName)
        {
            GroupName = groupName;
        }
    }
}