using AutoMapper;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.ServiceInterfaces;
using eios_translation.infrastructure.DbContext;
using eios_translation.infrastructure.EntityClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.infrastructure.ServiceImplementation
{
    public class LanguageService : ILanguageService
    {
        private readonly EIOSTranslationContext context;
        private readonly IMapper mapper;

        public LanguageService(EIOSTranslationContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<LanguageViewModel>> GetAllLanguages()
        {
            var Languages = await this.context.Languages.AsNoTracking().ToListAsync();
            return this.mapper.Map<List<LanguageViewModel>>(Languages);
        }

        public LanguageViewModel GetSelectedLanguage(int languageId)
        {
            var result =  this.context.Languages.First(a => a.LanguageId == languageId);
            return this.mapper.Map<LanguageViewModel>(result);
        }

        public void InsertLanguage(LanguageViewModel language)
        {
             this.context.Languages.Add(this.mapper.Map<Language>(language));
        }

        public void UpdateLanguage(LanguageViewModel language)
        {
            this.context.Languages.Update(this.mapper.Map<Language>(language));
        }
    }
}
