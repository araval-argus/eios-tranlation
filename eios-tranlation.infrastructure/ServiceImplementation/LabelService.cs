using AutoMapper;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_tranlation.infrastructure.ServiceImplementation;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.ServiceInterfaces;
using eios_translation.infrastructure.DbContext;
using eios_translation.infrastructure.EntityClass;
using Google.Cloud.Translation.V2;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Label = eios_translation.infrastructure.EntityClass.Label;

namespace eios_translation.infrastructure.ServiceImplementation
{
    public class LabelService : ILabelService
    {
        private readonly EIOSTranslationContext context;
        private readonly IMapper mapper;

        public LabelService(EIOSTranslationContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<LabelViewModel>> GetAllLabels(int languageId)
        {
            var labels = await this.context.Labels.AsNoTracking().ToListAsync();
            return this.mapper.Map<List<LabelViewModel>>(labels);
        }

        public async Task<LabelViewModel> GetSelectedLabel(int LabelId)
        {
            var result = await this.context.Labels.FirstAsync(a => a.LabelId == LabelId);
            return this.mapper.Map<LabelViewModel>(result);
        }

        public async Task<int> InsertLabel(LabelViewModel label, string endpoint, string key, string location)
        {
            try
            {
                
                var languages = await this.context.Languages.AsNoTracking().ToListAsync();
                foreach (var language in languages)
                {
                    Label label1 = this.mapper.Map<Label>(label);
                    string translation = await GetTranslatedStringAsync(label1.LabelValue, "en", language.LanguageCode, key, endpoint, location);
                    label1.MachineTranslation = translation;
                    label1.FK_LanguageId = language.LanguageId;
                    var result = context.Labels.Add(label1);
                    await context.SaveChangesAsync();
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> UpdateLabel(LabelViewModel label)
        {
            try
            {
                var result = context.Labels.Update(this.mapper.Map<Label>(label));
                await context.SaveChangesAsync();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        
        public async Task<string> GetTranslatedStringAsync(string LabelValue,string SourceLanguage,string TargetLanguage,string key,string endpoint,string location)
        {
            LanguageService languageService = new LanguageService(context, mapper);
            string translation = await languageService.AzureTranslate(LabelValue, SourceLanguage, TargetLanguage, key, endpoint, location);
            JArray a = JArray.Parse(translation);
            foreach (JObject o in a.Children<JObject>())
            {
                foreach (JProperty p in o.Properties())
                {
                    foreach (JObject s in p.Value.Children<JObject>())
                    {
                        var RootObjects = JsonConvert.DeserializeObject<TranslatedText>(s.ToString());
                        translation = RootObjects.Text;
                    }
                }
            }
            return translation;
        }
    }
    public class TranslatedText
    {
        public string? Text { get; set; }
        public string? To { get; set; }
    }
}
