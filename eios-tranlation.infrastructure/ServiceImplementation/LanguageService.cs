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

        public async Task<LanguageViewModel> GetSelectedLanguage(int languageId)
        {
            var result = await this.context.Languages.FirstAsync(a => a.LanguageId == languageId);
            return this.mapper.Map<LanguageViewModel>(result);
        }

        public async Task<int> InsertLanguage(LanguageViewModel language)
        {
            try
            {
                this.context.Languages.Add(this.mapper.Map<Language>(language));
                await context.SaveChangesAsync();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> UpdateLanguage(LanguageViewModel language)
        {
            try
            {
                this.context.Languages.Update(this.mapper.Map<Language>(language));
                await context.SaveChangesAsync();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }
}
