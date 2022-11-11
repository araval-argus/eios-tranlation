using eios_tranlation.businesslogic.Features.Label;
using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.MediatRPiplelineBehavior;
using eios_translation.businesslogic.ServiceInterfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.businesslogic.Features.Label
{
    public class GetSelectedLabelCommand : Request<LabelViewModel>, IGetEntityCommand<LabelViewModel>
    {
        public int LabelId { get; set; }
    }

    /// <summary>
    /// Validation class for MediatR <see cref="GetSelectedLabelCommand"/>.
    /// </summary>
    public class GetSelectedLabelCommandValidator : AbstractValidator<GetSelectedLabelCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSelectedLabelCommandValidator"/> class.
        /// </summary>
        public GetSelectedLabelCommandValidator()
        {
            this.RuleFor(x => x.LabelId).GreaterThan(0);
        }
    }
    /// <summary>
    /// Authorization for MediatR <see cref="GetSelectedLabelCommand"/> Query.
    /// </summary>
    public class GetSelectedLabelCommandAuthorization : IAuthorize<GetSelectedLabelCommand, LabelViewModel>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GetSelectedLabelCommand, LabelViewModel>.Authorize(GetSelectedLabelCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Handler class for the MediatR <see cref="GetAllQuestionTypesCommand"/> command.
    /// </summary>
    public class GetSelectedLabelCommandHandler : IGetEntityCommandHandler<GetSelectedLabelCommand, LabelViewModel>
    {
        private readonly ILabelService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSelectedLabelCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILabelService "/> service.</param>
        public GetSelectedLabelCommandHandler(ILabelService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<LabelViewModel> Handle(GetSelectedLabelCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GetSelectedLabel(request.LabelId);
        }
    }
}
