using eios_tranlation.businesslogic.Features.LabelGroup;
using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.MediatRPiplelineBehavior;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.businesslogic.Features.LabelGroup
{
    public class GetSelectedLabelGroupCommand : Request<LabelGroupViewModel>, IGetEntityCommand<LabelGroupViewModel>
    {
        public int LabelGroupId { get; set; }
    }
    /// <summary>
    /// Validation class for MediatR <see cref="GetSelectedLabelGroupCommand"/>.
    /// </summary>
    public class GetSelectedLabelGroupCommandValidator : AbstractValidator<GetSelectedLabelGroupCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSelectedLabelGroupCommandValidator"/> class.
        /// </summary>
        public GetSelectedLabelGroupCommandValidator()
        {
            this.RuleFor(x => x.LabelGroupId).GreaterThan(0);
        }
    }
    /// <summary>
    /// Authorization for MediatR <see cref="GetSelectedLabelGroupCommand"/> Query.
    /// </summary>
    public class GetSelectedLabelGroupCommandAuthorization : IAuthorize<GetSelectedLabelGroupCommand, LabelGroupViewModel>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GetSelectedLabelGroupCommand, LabelGroupViewModel>.Authorize(GetSelectedLabelGroupCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Handler class for the MediatR <see cref="GetAllQuestionTypesCommand"/> command.
    /// </summary>
    public class GetSelectedLabelGroupCommandHandler : IGetEntityCommandHandler<GetSelectedLabelGroupCommand, LabelGroupViewModel>
    {
        private readonly ILabelGroupService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSelectedLabelGroupCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILabelGroupService "/> service.</param>
        public GetSelectedLabelGroupCommandHandler(ILabelGroupService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<LabelGroupViewModel> Handle(GetSelectedLabelGroupCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GetSelectedLabelGroup(request.LabelGroupId);
        }
    }
}
