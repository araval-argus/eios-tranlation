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
    public class GetParentLabelGroupsCommand : Request<List<LabelGroupViewModel>>, IGetEntityCommand<List<LabelGroupViewModel>>
    {
    }

    /// <summary>
    /// Validation class for MediatR <see cref="GetParentLabelGroupsCommand"/>.
    /// </summary>
    public class GetParentLabelGroupsCommandValidator : AbstractValidator<GetParentLabelGroupsCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetParentLabelGroupsCommandValidator"/> class.
        /// </summary>
        public GetParentLabelGroupsCommandValidator()
        {
        }
    }

    /// <summary>
    /// Authorization for MediatR <see cref="GetParentLabelGroupsCommand"/> Query.
    /// </summary>
    public class GetParentLabelGroupsCommandAuthorization : IAuthorize<GetParentLabelGroupsCommand, List<LabelGroupViewModel>>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GetParentLabelGroupsCommand, List<LabelGroupViewModel>>.Authorize(GetParentLabelGroupsCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Handler class for the MediatR <see cref="GetAllQuestionTypesCommand"/> command.
    /// </summary>
    public class GetParentLabelGroupsCommandHandler : IGetEntityCommandHandler<GetParentLabelGroupsCommand, List<LabelGroupViewModel>>
    {
        private readonly ILabelGroupService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetParentLabelGroupsCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILabelGroupService "/> service.</param>
        public GetParentLabelGroupsCommandHandler(ILabelGroupService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        async Task<List<LabelGroupViewModel>> IRequestHandler<GetParentLabelGroupsCommand, List<LabelGroupViewModel>>.Handle(GetParentLabelGroupsCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GetAllLabelGroups(true);
        }
    }
}

