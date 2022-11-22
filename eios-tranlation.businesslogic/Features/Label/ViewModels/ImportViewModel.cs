using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.businesslogic.Features.Label.ViewModels
{
    public class ImportViewModel
    {
        public List<ImportLabelGroup> ImportLabelGroups { get; set; } = new List<ImportLabelGroup>();
    }
    public class ImportLabelGroup
    {
        public int LabelGroupId { get; set; }

        public string GroupName { get; set; }

        public List<ImportLabels> Labels { get; set; } = new List<ImportLabels>();

        public List<ImportLabelGroup> ChildGroups { get; set; } = new List<ImportLabelGroup>();

    }
    public class ImportLabels
    {
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        public string LabelValue { get; set; }
    }
}
