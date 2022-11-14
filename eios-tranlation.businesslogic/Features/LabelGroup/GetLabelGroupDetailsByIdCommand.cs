using eios_tranlation.businesslogic.Features.LabelGroup;
using eios_tranlation.businesslogic.Features.LabelGroup.ViewModels;
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
    public class GetLabelGroupDetailsByIdCommand : Request<LabelGroupDetailViewModel>, IGetEntityCommand<LabelGroupDetailViewModel>
    {
        public int LabelGroupId { get; set; }
    }
    /// <summary>
    /// Validation class for MediatR <see cref="GetLabelGroupDetailsByIdCommand"/>.
    /// </summary>
    public class GetLabelGroupDetailsByIdCommandValidator : AbstractValidator<GetLabelGroupDetailsByIdCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLabelGroupDetailsByIdCommandValidator"/> class.
        /// </summary>
        public GetLabelGroupDetailsByIdCommandValidator()
        {
            this.RuleFor(x => x.LabelGroupId).GreaterThan(0);
        }
    }
    /// <summary>
    /// Authorization for MediatR <see cref="GetLabelGroupDetailsByIdCommand"/> Query.
    /// </summary>
    public class GetLabelGroupDetailsByIdCommandAuthorization : IAuthorize<GetLabelGroupDetailsByIdCommand, LabelGroupDetailViewModel>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GetLabelGroupDetailsByIdCommand, LabelGroupDetailViewModel>.Authorize(GetLabelGroupDetailsByIdCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Handler class for the MediatR <see cref="GetAllQuestionTypesCommand"/> command.
    /// </summary>
    public class GetLabelGroupDetailsByIdCommandHandler : IGetEntityCommandHandler<GetLabelGroupDetailsByIdCommand, LabelGroupDetailViewModel>
    {
        private readonly ILabelGroupService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLabelGroupDetailsByIdCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILabelGroupService "/> service.</param>
        public GetLabelGroupDetailsByIdCommandHandler(ILabelGroupService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<LabelGroupDetailViewModel> Handle(GetLabelGroupDetailsByIdCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GetLabelGroupDetailsById(request.LabelGroupId);
        }
    }
}
