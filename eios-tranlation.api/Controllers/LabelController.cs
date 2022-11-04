using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_tranlation.infrastructure.ServiceImplementation;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.ServiceInterfaces;
using eios_translation.core.Common;
using eios_translation.core.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eios_translation.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {

        private readonly ILogger<LabelController> logger;
        private readonly ILabelService labelService;
        private readonly IConfiguration configuration;

        public LabelController(ILogger<LabelController> logger, ILabelService labelService, IConfiguration _configuration)
        {
            this.logger = logger;
            this.labelService = labelService;
            this.configuration = _configuration;
        }

        /// <summary>
        /// Api to Get All Labels.
        /// </summary>
        /// <param name="languageId">LanguageId.</param>
        /// <returns>The list of <see cref="QuestionnaireViewModel"/> model.</returns>
        /// <exception cref="ApiException">Invalid fields values.</exception>
        [HttpGet("GetAllLabels")]
        [ProducesResponseType(typeof(ApiResponse<List<LabelViewModel>>), 200)]
        public async Task<IActionResult> GetAllLabels(int languageId)
        {
            return this.Ok(await this.labelService.GetAllLabels(languageId));
        }
        /// <summary>
        /// API to get Selected Label
        /// </summary>
        /// <param name="LabelId"></param>
        /// <returns></returns>
        [HttpGet("GetSelectedLabel")]
        [ProducesResponseType(typeof(LabelViewModel), 200)]
        public async Task<IActionResult> GetSelectedLabel(int LabelId)
        {
            return this.Ok(await this.labelService.GetSelectedLabel(LabelId));
        }
        [HttpPost("InsertLabel")]
        public async Task<IActionResult> InsertLabelAsync()
        {
            try
            {
                LabelViewModel label = new LabelViewModel();
                //label.LabelId = 10;
                label.ResourceId = "2";
                label.FK_LabelGroupId = 3;
                label.FK_LanguageId = 4;
                label.LabelValue = "How are you";
                label.LabelType = LabelType.PreLabel;
                label.LabelDescription = "New Label2";
                label.LabelSnapshotPath = "TestPath";
                label.MachineTranslation = "Test";
                label.Scope = "1";
                label.TranslationStatus = TranslationStatus.Published;
                label.Version = 1;
                label.IsActive = true;
                label.FK_PrevVersionLabelId = 0;
                string key = configuration.GetValue<string>("key");
                string endpoint = configuration.GetValue<string>("endpoint");
                string location = configuration.GetValue<string>("location");
                if (await this.labelService.InsertLabel(label, endpoint, key, location) == 1)
                {
                    return this.Ok(label);
                }
                else
                {
                    return this.NotFound();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        //[HttpPost("InsertLabel")]
        //public async Task<IActionResult> InsertLabelAsync(LabelViewModel label)
        //{
        //    List<LanguageViewModel> AllLanguages = await this.languageService.GetAllLanguages();
        //    foreach(LanguageViewModel language in AllLanguages)
        //    {

        //    }
        //    if (await this.labelService.InsertLabel(label) == 1)
        //    {
        //        return this.Ok(label);
        //    }
        //    else
        //    {
        //        return this.NotFound();
        //    }
        //}

        [HttpPost("UpdateLabel")]
        [ProducesResponseType(typeof(LabelViewModel), 200)]
        public async Task<IActionResult> UpdateLabel(LabelViewModel label)
        {
            if (await this.labelService.UpdateLabel(label) == 1)
            {
                return this.Ok(label);
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}
