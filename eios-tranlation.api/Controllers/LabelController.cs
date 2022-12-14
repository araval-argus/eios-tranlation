using eios_tranlation.businesslogic.Features.Label;
using eios_tranlation.businesslogic.Features.LabelGroup;
using eios_tranlation.businesslogic.Features.Language;
using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_tranlation.core.Constants;
using eios_tranlation.infrastructure.ServiceImplementation;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.ServiceInterfaces;
using eios_translation.core.Common;
using eios_translation.core.Wrappers;
using MediatR;
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
        private readonly IMediator mediator;

        public LabelController(ILogger<LabelController> logger, ILabelService labelService, IConfiguration _configuration, IMediator mediator)
        {
            this.logger = logger;
            this.labelService = labelService;
            this.configuration = _configuration;
            this.mediator = mediator;
        }

        ///// <summary>
        ///// Api to Get All Labels.
        ///// </summary>
        ///// <param name="languageId">LanguageId.</param>
        ///// <returns>The list of <see cref="QuestionnaireViewModel"/> model.</returns>
        ///// <exception cref="ApiException">Invalid fields values.</exception>
        //[HttpGet("GetAllLabels")]
        //[ProducesResponseType(typeof(ApiResponse<List<LabelViewModel>>), 200)]
        //public async Task<IActionResult> GetAllLabels(int languageId)
        //{
        //    return this.Ok(await this.labelService.GetAllLabels(languageId));
        //}

        /// <summary>
        /// Api to Get All Label.
        /// </summary>
        /// <exception cref="ApiException">Invalid fields values.</exception>
        [HttpGet("GetAllLabels")]
        [ProducesResponseType(typeof(ApiResponse<List<LabelViewModel>>), 200)]
        public async Task<IActionResult> GetAllLabels()
            => this.Ok(await this.mediator.Send(new GetAllLabelsCommand()));

        /// <summary>
        /// Api to Get selected Language.
        /// </summary>
        /// <exception cref="ApiException">Invalid fields values.</exception>
        [HttpGet("GetSelectedLabel/{labelId:int}")]
        [ProducesResponseType(typeof(ApiResponse<LabelViewModel>), 200)]
        public async Task<IActionResult> GetSelectedLabel([FromRoute] int labelId)
                   => this.Ok(await this.mediator.Send(new GetSelectedLabelCommand { LabelId = labelId }));

        [HttpPost("InsertLabel")]
        public async Task<IActionResult> InsertLabel(InsertLabelCommand request)
         => this.Ok(await this.mediator.Send(request));

        [HttpPost("UpdateLabel")]
        [ProducesResponseType(typeof(LabelViewModel), 200)]
        public async Task<IActionResult> UpdateLabel([FromQuery] int labelId, [FromBody] UpdateLabelCommand request)
        {
            request.LabelId = labelId;
            return this.Ok(await this.mediator.Send(request));
        }

        [HttpGet("ExportLabelsByLanguageId")]
        [ProducesResponseType(typeof(ApiResponse<LabelViewModel>), 200)]
        public async Task<IActionResult> ExportLabelsByLanguageId([FromQuery] string languageCode, [FromQuery] int? labelGroupId = 0)
        {
            if (labelGroupId.GetValueOrDefault() > 0)
            {
                return this.Ok(await this.mediator.Send(new ExportLabelsByLanguageAndGroupCommand { LanguageCode = languageCode, LabelGroupId = labelGroupId.Value }));
            }
            else
            {
                return this.Ok(await this.mediator.Send(new ExportLabelsByLanguageIdCommand { LanguageCode = languageCode }));
            }
        }

        [HttpPost("ImportLabelsByLanguageId")]
        [ProducesResponseType(typeof(ApiResponse<LabelViewModel>), 200)]
        public async Task<IActionResult> ImportLabelsByLanguageId([FromQuery] string languageCode, IFormFile file, [FromQuery] int? labelGroupId = 0)
        {
            if (labelGroupId.GetValueOrDefault() > 0)
            {
                return this.Ok(await this.mediator.Send(new ImportLabelsByLanguageAndGroupCommand { LanguageCode = languageCode, LabelGroupId = labelGroupId.Value, File = file }));
            }
            else
            {
                return this.Ok(await this.mediator.Send(new ImportLabelsByLanguageIdCommand { LanguageCode = languageCode, File = file }));
            }
        }
        
    }
}
