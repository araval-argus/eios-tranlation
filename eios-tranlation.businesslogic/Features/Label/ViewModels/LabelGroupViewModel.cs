using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_translation.businesslogic.Features.Label.ViewModels
{
    public class LabelGroupViewModel
    {
        public int LabelGroupId { get; set; }
        public string GroupName { get; set; }
        public int? FK_ParentLableGroupId { get; set; }
        public LabelGroupViewModel ParentGroup { get; set; }
    }
}
