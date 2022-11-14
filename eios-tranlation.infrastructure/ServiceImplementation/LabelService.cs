using AutoMapper;
using eios_tranlation.businesslogic.Features.Label;
using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_tranlation.core.Constants;
using eios_tranlation.infrastructure.ServiceImplementation;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.ServiceInterfaces;
using eios_translation.core.Common;
using eios_translation.core.Wrappers;
using eios_translation.infrastructure.DbContext;
using eios_translation.infrastructure.EntityClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Label = eios_translation.infrastructure.EntityClass.Label;

namespace eios_translation.infrastructure.ServiceImplementation
{
    public class LabelService : ILabelService
    {

        private readonly EIOSTranslationContext context;
        private readonly IMapper mapper;
        private readonly string key = CommonSettings.AzureTranslationSettings.Key;
        private readonly string endpoint = CommonSettings.AzureTranslationSettings.Endpoint;
        private readonly string location = CommonSettings.AzureTranslationSettings.Location;


        public LabelService(EIOSTranslationContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<LabelViewModel>> GetAllLabels()
        {
            var labels = await this.context.Labels.ToListAsync();
            return this.mapper.Map<List<LabelViewModel>>(labels);
        }

        public async Task<LabelViewModel> GetSelectedLabel(int LabelId)
        {
            var result = await this.context.Labels.FirstOrDefaultAsync(a => a.LabelId == LabelId);
            if (result == null)
            {
                throw new ApiException($"No label found with Id : {LabelId}");
            }
            return this.mapper.Map<LabelViewModel>(result);
        }

        public async Task<int> InsertLabel(InsertLabelCommand request)
        {
            try
            {
                // Get Default Language.
                Language baseLanguage = await this.context.Languages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.IsDefault);

                if (baseLanguage == null)
                {
                    throw new ApiException($"Please add default english language first");
                }

                Label baseLabel = new Label(
                    resourceid: request.ResourceId,
                    fk_labelgroupid: request.FK_LabelGroupId,
                    fk_languageid: baseLanguage.LanguageId,
                    labelvalue: request.LabelValue,
                    labeltype: LabelType.Normal,
                    labeldescription: null,
                    labelsnapshotpath: null);

                // Get Non-Default Language.
                context.Labels.Add(baseLabel);
                await context.SaveChangesAsync();

                //var languages = await this.context.Languages.AsNoTracking().Where(x=>!x.IsDefault).ToListAsync();
                //foreach (var language in languages)
                //{
                //    Label tranlatedLabel = new Label(
                //    resourceid: request.ResourceId,
                //    fk_labelgroupid: request.FK_LabelGroupId,
                //    fk_languageid: baseLanguage.LanguageId,
                //    labelvalue: request.LabelValue,
                //    labeltype: LabelType.Normal,
                //    labeldescription: null,
                //    labelsnapshotpath: null);

                //    string translation = await GetTranslatedStringAsync(tranlatedLabel.LabelValue, "en", language.LanguageCode, key, endpoint, location);
                //    tranlatedLabel.SetMachineTranslation (language.LanguageId, translation);
                //    context.Labels.Add(tranlatedLabel);
                //    await context.SaveChangesAsync();

                //}
                return 1;
            }
            catch(Exception ex)
            {
                throw new ApiException($"Something went wrong while adding the label: {ex.Message}");
            }
        }

        public async Task<LabelViewModel> UpdateLabel(UpdateLabelCommand request)
        {
            try
            {
                var dbLabel = await this.context.Labels.FirstOrDefaultAsync(x => x.LabelId == request.LabelId);
                if (dbLabel == null)
                {
                    throw new ApiException($"No Label found with Id:  {request.LabelId}");
                }
                

                var result = context.Labels.Update(dbLabel);
                await context.SaveChangesAsync();
                return this.mapper.Map<LabelViewModel>(dbLabel);
            }
            catch(Exception ex)
            {
                throw new ApiException($"Something went wrong while updating the label: {ex.Message}");
            }
        }
        
        public async Task<string> GetTranslatedStringAsync(string LabelValue,string SourceLanguage,string TargetLanguage,string key,string endpoint,string location)
        {
            LanguageService languageService = new LanguageService(context, mapper);
            string translation = await languageService.AzureTranslate(LabelValue, SourceLanguage, TargetLanguage);
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
