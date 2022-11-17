using eios_tranlation.businesslogic.Features.LabelGroup;
using eios_tranlation.businesslogic.Features.LabelGroup.ViewModels;
using eios_tranlation.businesslogic.Features.Language;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.core.Wrappers;
using eios_translation.infrastructure.EntityClass;
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
            this.mediator = mediator;
        }

        /// <summary>
        /// Api to Get All Parent Groups.
        /// </summary>
        /// <exception cref="ApiException">Invalid fields values.</exception>
        [HttpGet("GetParentLabelGroups")]
        [ProducesResponseType(typeof(ApiResponse<List<LabelGroupViewModel>>), 200)]
        public async Task<IActionResult> GetParentLabelGroupsCommand()
            => this.Ok(await this.mediator.Send(new GetParentLabelGroupsCommand()));

        /// <summary>
        /// Api to Get Group Details.
        /// </summary>
        /// <exception cref="ApiException">Invalid fields values.</exception>
        [HttpGet("GetLabelGroupDetailsById/{labelGroupId:int}")]
        [ProducesResponseType(typeof(ApiResponse<LabelGroupDetailViewModel>), 200)]
        public async Task<IActionResult> GetLabelGroupDetailsById([FromRoute] int labelGroupId)
            => this.Ok(await this.mediator.Send(new GetLabelGroupDetailsByIdCommand { LabelGroupId = labelGroupId }));

        /// <summary>
        /// Api to Save Group Details.
        /// </summary>
        /// <exception cref="ApiException">Invalid fields values.</exception>
        [HttpPost("SaveLabelGroupDetailsById")]
        [ProducesResponseType(typeof(ApiResponse<LabelGroupDetailViewModel>), 200)]
        public async Task<IActionResult> SaveLabelGroupDetailsById([FromBody] SaveLabelGroupDetailsByIdCommand request)
        => this.Ok(await this.mediator.Send(request));
        

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
        [HttpGet("GetSelectedLabelGroup/{labelGroupId:int}")]
        [ProducesResponseType(typeof(ApiResponse<LabelGroupViewModel>), 200)]
        public async Task<IActionResult> GetSelectedLabelGroup([FromRoute] int labelGroupId)
                   => this.Ok(await this.mediator.Send(new GetSelectedLabelGroupCommand { LabelGroupId = labelGroupId }));

        [HttpPost("InsertLabelGroup")]
        public async Task<IActionResult> InsertLabelGroup(InsertLabelGroupCommand request)
                 => this.Ok(await this.mediator.Send(request));

        [HttpPost("UpdateLabelGroup/{labelGroupId:int}")]
        [ProducesResponseType(typeof(LabelGroupViewModel), 200)]
        public async Task<IActionResult> UpdateLabelGroup([FromRoute] int labelGroupId, [FromBody] UpdateLabelGroupCommand request)
        {
            request.LabelGroupId = labelGroupId;
            return this.Ok(await this.mediator.Send(request));
        } 
    }
}
