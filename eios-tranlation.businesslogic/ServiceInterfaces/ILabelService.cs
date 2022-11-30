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
        Task<List<LabelViewModel>> GetAllLabels();
        Task<LabelViewModel> GetSelectedLabel(int LabelId);
        Task<LabelViewModel> UpdateLabel(UpdateLabelCommand labelgroup);
        Task<int> InsertLabel(InsertLabelCommand labelgroup);
        Task<string> ExportLabelsByLanguageId(string languageCode);
        Task<bool> ImportLabelsByLanguageId(ImportLabelsByLanguageIdCommand request);
        Task<string> ExportLabelsByGroupId(int languageId, int GroupId);

        Task<string> ExportLabelsByLanguageAndGroup(string languageCode, int labelGroupId);
    }
}
