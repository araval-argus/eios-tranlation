using eios_translation.businesslogic.Features.Label.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.businesslogic.ServiceInterfaces
{
    public interface ILabelGroupService
    {
        Task<List<LabelGroupViewModel>> GetAllLabelGroups();
        LabelGroupViewModel GetSelectedLabelGroup(int LabelGroupId);
        void UpdateLabelGroup(LabelGroupViewModel labelgroup);
        void InsertLabelGroup(LabelGroupViewModel labelgroup);
    }
}
