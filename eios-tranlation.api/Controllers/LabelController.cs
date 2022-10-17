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
    public class LabelController : ControllerBase
    {
        
        private readonly ILogger<LabelController> logger;
        private readonly ILabelService labelService;
        public LabelController(ILogger<LabelController> logger, ILabelService labelService)
        {
            this.logger = logger;
            this.labelService = labelService;
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
        [HttpGet("GetSelectedLabel")]
        [ProducesResponseType(typeof(LabelViewModel), 200)]
        public IActionResult GetSelectedLabel(int LabelId)
        {
            return this.Ok(this.labelService.GetSelectedLabel(LabelId));
        }

        [HttpPost("InsertLabel")]
        public IActionResult InsertLabel(LabelViewModel label)
        {
            if (this.labelService.InsertLabel(label) == 1)
            {
                return this.Ok(label);
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpGet("UpdateLabel")]
        public IActionResult UpdateLabel(LabelViewModel label)
        {
            if (this.labelService.UpdateLabel(label) == 1)
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
