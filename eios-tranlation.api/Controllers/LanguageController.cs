using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.ServiceInterfaces;
using eios_translation.core.Wrappers;
using eios_translation.infrastructure.DbContext;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;
using Microsoft.Extensions.Configuration;
using MediatR;
using eios_tranlation.businesslogic.Features.Language;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.core.Constants;

namespace eios_translation.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {

        private readonly ILogger<LanguageController> logger;
        private readonly ILanguageService languageService;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;

        public LanguageController(ILogger<LanguageController> logger, ILanguageService languageService, IConfiguration _configuration, IMediator mediator)
        {
            this.logger = logger;
            this.languageService = languageService;
            this.configuration = _configuration;
            this.mediator = mediator;
        }

        /// <summary>
        /// Api to Get All Languages.
        /// </summary>
        /// <exception cref="ApiException">Invalid fields values.</exception>
        [HttpGet("GetAllLanguages")]
        [ProducesResponseType(typeof(ApiResponse<List<LanguageViewModel>>), 200)]
        public async Task<IActionResult> GetAllLanguages()
            => this.Ok(await this.mediator.Send(new GetAllLanguagesCommand()));

        /// <summary>
        /// Translate Source text from Source language to target language using Google API
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sourceLanguage"></param>
        /// <param name="targetLanguage"></param>
        /// <returns></returns>
        [HttpGet("GoogleTranslateText")]
        public async Task<IActionResult> GoogleTranslate(string source, string sourceLanguage, string targetLanguage)
         => this.Ok(await this.mediator.Send(new GoogleTranslateTextCommand { Source = source, TargetLanguage = targetLanguage, SourceLanguage = sourceLanguage }));

        /// <summary>
        /// Translate Source text from Source language to target language using Azure API
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sourceLanguage"></param>
        /// <param name="targetLanguage"></param>
        /// <returns></returns>
        [HttpGet("AzureTranslateText")]
        public async Task<IActionResult> AzureTranslate(string source, string sourceLanguage, string targetLanguage)
            => this.Ok(await this.mediator.Send(new AzureTranslateTextCommand { Source = source, TargetLanguage = targetLanguage, SourceLanguage = sourceLanguage }));
        //{
        //    string key = CommonSettings.AzureTranslationSettings.Key;
        //    string endpoint = CommonSettings.AzureTranslationSettings.Endpoint;
        //    string location = CommonSettings.AzureTranslationSettings.Location;
        //    return (await languageService.AzureTranslate(Source, sourceLanguage, targetLanguage, key, endpoint, location));
        //}

        /// <summary>
        /// Api to Get selected Language.
        /// </summary>
        /// <exception cref="ApiException">Invalid fields values.</exception>
        [HttpGet("GetSelectedLanguage/{languageId:int}")]
        [ProducesResponseType(typeof(ApiResponse<List<LanguageViewModel>>), 200)]
        public async Task<IActionResult> GetSelectedLanguage([FromRoute] int languageId)
                   => this.Ok(await this.mediator.Send(new GetSelectedLanguagesCommand { LanguageId = languageId }));


        [HttpPost("InsertLanguage")]
        public async Task<IActionResult> InsertLanguage(InsertLanguageCommand request)
                 => this.Ok(await this.mediator.Send(request));

        [HttpPost("UpdateLanguage/{languageId:int}")]
        [ProducesResponseType(typeof(LanguageViewModel), 200)]
        public async Task<IActionResult> UpdateLanguage([FromRoute] int languageId,[FromBody] UpdateLanguageCommand request)
        {
            request.LanguageId = languageId;
            return this.Ok(await this.mediator.Send(request));
        }

    }
}
