namespace eios_tranlation.businesslogic.Features.LabelGroup
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
    using eios_tranlation.businesslogic.ServiceInterfaces;
    using eios_translation.businesslogic.Features.Label.ViewModels;
    using eios_translation.businesslogic.MediatRPiplelineBehavior;
    using eios_translation.businesslogic.ServiceInterfaces;
    using FluentValidation;
    using MediatR;

    /// <summary>
    /// GetAllLabelGroups MediatR Command.
    /// </summary>
    public class GetAllLabelGroupsCommand : Request<List<LabelGroupViewModel>>, IGetEntityCommand<List<LabelGroupViewModel>>
    {
        public int LabelGroupId { get; set; }
    }

    /// <summary>
    /// Validation class for MediatR <see cref="GetAllLabelGroupsCommand"/>.
    /// </summary>
    public class GetAllLabelGroupsCommandValidator : AbstractValidator<GetAllLabelGroupsCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllLabelGroupsCommandValidator"/> class.
        /// </summary>
        public GetAllLabelGroupsCommandValidator()
        {
        }
    }

    /// <summary>
    /// Authorization for MediatR <see cref="GetAllLabelGroupsCommand"/> Query.
    /// </summary>
    public class GetAllLabelGroupsCommandAuthorization : IAuthorize<GetAllLabelGroupsCommand, List<LabelGroupViewModel>>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GetAllLabelGroupsCommand, List<LabelGroupViewModel>>.Authorize(GetAllLabelGroupsCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Handler class for the MediatR <see cref="GetAllQuestionTypesCommand"/> command.
    /// </summary>
    public class GetAllLabelGroupsCommandHandler : IGetEntityCommandHandler<GetAllLabelGroupsCommand, List<LabelGroupViewModel>>
    {
        private readonly ILabelGroupService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllLabelGroupsCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILabelGroupService "/> service.</param>
        public GetAllLabelGroupsCommandHandler(ILabelGroupService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        async Task<List<LabelGroupViewModel>> IRequestHandler<GetAllLabelGroupsCommand, List<LabelGroupViewModel>>.Handle(GetAllLabelGroupsCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GetAllLabelGroups();
        }
    }
}

