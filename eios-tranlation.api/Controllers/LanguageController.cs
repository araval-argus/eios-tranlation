using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.ServiceInterfaces;
using eios_translation.core.Wrappers;
using eios_translation.infrastructure.DbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;


namespace eios_translation.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {

        private readonly ILogger<LanguageController> logger;
        private readonly ILanguageService languageService;
        public LanguageController(ILogger<LanguageController> logger, ILanguageService languageService)
        {
            this.logger = logger;
            this.languageService = languageService;
        }

        /// <summary>
        /// Api to Get All Languages.
        /// </summary>
        /// <exception cref="ApiException">Invalid fields values.</exception>
        [HttpGet("GetAllLanguages")]
        [ProducesResponseType(typeof(ApiResponse<List<LanguageViewModel>>), 200)]
        public async Task<IActionResult> GetAllLanguages()
        {
            return this.Ok(await this.languageService.GetAllLanguages());
        }

        /// <summary>
        /// Api to Get selected Language.
        /// </summary>
        /// <exception cref="ApiException">Invalid fields values.</exception>
        [HttpGet("GetSelectedLanguages")]
        [ProducesResponseType(typeof(ApiResponse<List<LanguageViewModel>>), 200)]
        public IActionResult GetSelectedLanguage(int languageId)
        {
            return this.Ok(this.languageService.GetSelectedLanguage(languageId));
        }
        [HttpPost("InsertLanguage")]
        public IActionResult InsertLanguage(LanguageViewModel language)
        {
            if (this.languageService.InsertLanguage(language) == 1)
            {
                return this.Ok(language);
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpGet("UpdateLanguage")]
        public IActionResult UpdateLanguage(LanguageViewModel language)
        {
            if (this.languageService.UpdateLanguage(language) == 1)
            {
                return this.Ok(language);
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}
