using eios_tranlation.businesslogic.Features.LabelGroup;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.businesslogic.ServiceInterfaces
{
    public interface ILabelGroupService : IBaseService
    {
        Task<List<LabelGroupViewModel>> GetAllLabelGroups();
        Task<LabelGroupViewModel> GetSelectedLabelGroup(int LabelGroupId);
        Task<int> UpdateLabelGroup(UpdateLabelGroupCommand labelgroup);
        Task<int> InsertLabelGroup(InsertLabelGroupCommand labelgroup);
    }
}
