using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.api.Controllers;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.ServiceInterfaces;
using eios_translation.core.Wrappers;
using eios_translation.infrastructure.DbContext;
using eios_translation.infrastructure.EntityClass;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net;
using static System.Collections.Specialized.BitVector32;

namespace eios_tranlation.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelGroupController : ControllerBase
    {
        private readonly ILogger<LabelGroupController> logger;
        private readonly ILabelGroupService LabelGroupService;

        public LabelGroupController(ILogger<LabelGroupController> logger, ILabelGroupService labelGroupService)
        {
            this.logger = logger;
            this.LabelGroupService = labelGroupService;
        }

        /// <summary>
        /// Api to Get All LabelGroup.
        /// </summary>
        /// <exception cref="ApiException">Invalid fields values.</exception>
        [HttpGet("GetAllLabelGroups")]
        [ProducesResponseType(typeof(ApiResponse<List<LabelGroupViewModel>>), 200)]
        public async Task<IActionResult> GetAllLabelGroups()
        {
            return this.Ok(await this.LabelGroupService.GetAllLabelGroups());
        }

        /// <summary>
        /// Api to Get selected LabelGroup.
        /// </summary>
        /// <exception cref="ApiException">Invalid fields values.</exception>
        [HttpGet("GetSelectedLabelGroup")]
        [ProducesResponseType(typeof(ApiResponse<LanguageViewModel>), 200)]
        public IActionResult GetSelectedLabelGroup(int LabelGroupId)
        {
            return this.Ok(this.LabelGroupService.GetSelectedLabelGroup(LabelGroupId));
        }
        [HttpPost("InsertLabelGroup")]
        public async Task<IActionResult> InsertLabelGroup(LabelGroupViewModel labelgroup)
        {
            if (await this.LabelGroupService.InsertLabelGroup(labelgroup) == 1)
            {
                return this.Ok(labelgroup);
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpGet("UpdateLabelGroup")]
        public async Task<IActionResult> UpdateLabelGroup(LabelGroupViewModel labelgroup)
        {
            if (await this.LabelGroupService.UpdateLabelGroup(labelgroup) == 1)
            {
                return this.Ok(labelgroup);
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}
