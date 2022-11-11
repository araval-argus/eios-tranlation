using eios_tranlation.businesslogic.Features.Label;
using eios_translation.businesslogic.Features.Label.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_translation.businesslogic.ServiceInterfaces
{
    public interface ILabelService: IBaseService
    {
        Task<List<LabelViewModel>> GetAllLabels(int languageId);
        Task<LabelViewModel> GetSelectedLabel(int LabelId);
        Task<int> UpdateLabel(UpdateLabelCommand labelgroup);
        Task<int> InsertLabel(InsertLabelCommand labelgroup);
    }
}
