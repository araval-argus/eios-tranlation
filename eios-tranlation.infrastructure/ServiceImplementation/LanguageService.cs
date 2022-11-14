using AutoMapper;
using eios_tranlation.businesslogic.Features.Language;
using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_tranlation.core.Constants;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.core.Wrappers;
using eios_translation.infrastructure.DbContext;
using eios_translation.infrastructure.EntityClass;
using Google.Cloud.Translation.V2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using Language = eios_translation.infrastructure.EntityClass.Language;

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
            var result = await this.context.Languages.FirstOrDefaultAsync(a => a.LanguageId == languageId);
            if (result == null)
            {
                throw new ApiException($"No Language found with Id:  {languageId}.");
            }
            return this.mapper.Map<LanguageViewModel>(result);
        }

        public async Task<int> InsertLanguage(InsertLanguageCommand request)
        {
            try
            {
                Language language = new Language(name: request.Name, languageCode: request.LanguageCode, tolerance: request.Tolerance, toleranceType: request.ToleranceType, description: request.Description);
                this.context.Languages.Add(language);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApiException($"Something went wrong while creating the language: {ex.Message}");
            }
        }

        public async Task<LanguageViewModel> UpdateLanguage(UpdateLanguageCommand request)
        {
            try
            {
                var dbLanguage = await this.context.Languages.FirstOrDefaultAsync(x => x.LanguageId == request.LanguageId);
                if (dbLanguage == null)
                {
                    throw new ApiException($"No Language found with Id:  {request.LanguageId}");
                }
                dbLanguage.UpdateLanguage(name: request.Name, languageCode: request.LanguageCode, tolerance: request.Tolerance, toleranceType: request.ToleranceType, description: request.Description);
                await context.SaveChangesAsync();
                return this.mapper.Map<LanguageViewModel>(dbLanguage);
            }
            catch (Exception ex)
            {
                throw new ApiException($"Something went wrong while updating the language: {ex.Message}");
            }
        }

        public async Task<string> GoogleTranslate(string Source, string sourceLanguage, string targetLanguage)
        {
            var client = Google.Cloud.Translation.V2.TranslationClient.Create();
            var response = await client.TranslateTextAsync(Source, targetLanguage, sourceLanguage);
            return response.TranslatedText;
        }

        public async Task<string> AzureTranslate(string Source, string sourceLanguage, string targetLanguage)
        {
            string route = "/translate?api-version=3.0&from=" + sourceLanguage + "&to=" + targetLanguage;
            string textToTranslate = Source;
            object[] body = new object[] { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(CommonSettings.AzureTranslationSettings.Endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", CommonSettings.AzureTranslationSettings.Key);

                request.Headers.Add("Ocp-Apim-Subscription-Region", CommonSettings.AzureTranslationSettings.Location);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

                // Read response as a string.
                string result = await response.Content.ReadAsStringAsync();
           
                return (result);
            }
        }
    }
}