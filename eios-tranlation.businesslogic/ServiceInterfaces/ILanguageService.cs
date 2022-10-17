using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.ServiceInterfaces;

namespace eios_tranlation.businesslogic.ServiceInterfaces
{
    public interface ILanguageService : IBaseService
    {
        Task<List<LanguageViewModel>> GetAllLanguages();
        LanguageViewModel GetSelectedLanguage(int languageId);
        void UpdateLanguage(LanguageViewModel language);
        void InsertLanguage(LanguageViewModel language);
    }
}
