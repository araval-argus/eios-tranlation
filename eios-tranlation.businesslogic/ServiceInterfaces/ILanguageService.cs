using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.ServiceInterfaces;

namespace eios_tranlation.businesslogic.ServiceInterfaces
{
    public interface ILanguageService : IBaseService
    {
        Task<List<LanguageViewModel>> GetAllLanguages();
        Task<LanguageViewModel> GetSelectedLanguage(int languageId);
        Task<int> UpdateLanguage(LanguageViewModel language);
        Task<int> InsertLanguage(LanguageViewModel language);
    }
}
