using eios_tranlation.businesslogic.Features.LabelGroup;
using eios_tranlation.businesslogic.Features.Language;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.core.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eios_tranlation.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelGroupController : ControllerBase
    {
        private readonly ILogger<LabelGroupController> logger;
        private readonly ILabelGroupService LabelGroupService;
        private readonly IMediator mediator;

        public LabelGroupController(ILogger<LabelGroupController> logger, ILabelGroupService labelGroupService, IMediator mediator)
        {
            this.logger = logger;
            this.LabelGroupService = labelGroupService;
            this.mediator= mediator;
        }

        /// <summary>
        /// Api to Get All Label Groups.
        /// </summary>
        /// <exception cref="ApiException">Invalid fields values.</exception>
        [HttpGet("GetAllLabelGroups")]
        [ProducesResponseType(typeof(ApiResponse<List<LabelGroupViewModel>>), 200)]
        public async Task<IActionResult> GetAllLabelGroups()
            => this.Ok(await this.mediator.Send(new GetAllLabelGroupsCommand()));


        /// <summary>
        /// Api to Get selected LabelGroup.
        /// </summary>
        /// <exception cref="ApiException">Invalid fields values.</exception>
        [HttpGet("GetSelectedLabelGroup")]
        [ProducesResponseType(typeof(ApiResponse<LabelGroupViewModel>), 200)]
        public async Task<IActionResult> GetSelectedLabelGroup(int LabelGroupId)
        {
            //LabelGroupViewModel lgm = await this.LabelGroupService.GetSelectedLabelGroup(LabelGroupId);
            return this.Ok(await this.LabelGroupService.GetSelectedLabelGroup(LabelGroupId));
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

        [HttpPost("UpdateLabelGroup")]
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
