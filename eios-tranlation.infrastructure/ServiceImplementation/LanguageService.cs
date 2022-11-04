using AutoMapper;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.infrastructure.DbContext;
using eios_translation.infrastructure.EntityClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

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

        public async Task<string> GoogleTranslate(string Source, string sourceLanguage, string targetLanguage)
        {
            if (string.IsNullOrEmpty(Source))
                return "";

            var client = Google.Cloud.Translation.V2.TranslationClient.Create();
            
            var response = await client.TranslateTextAsync(Source, targetLanguage, sourceLanguage);
            return response.TranslatedText;
        }

        public async Task<string> AzureTranslate(string Source, string sourceLanguage, string targetLanguage,string key, string endpoint, string location)
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
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);

                request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

                // Read response as a string.
                string result = await response.Content.ReadAsStringAsync();
           
                return (result);
            }
        }
    }
}