namespace eios_tranlation.businesslogic.Features.Label
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

    /// <summary>
    /// GetAllLabelsCommand MediatR Command.
    /// </summary>
    public class GetAllLabelsCommand : Request<List<LabelViewModel>>, IGetEntityCommand<List<LabelViewModel>>
    {
        public int LanguageId { get; set; }
    }

    /// <summary>
    /// Validation class for MediatR <see cref="GetAllLabelsCommand"/>.
    /// </summary>
    public class GetAllLabelsCommandValidator : AbstractValidator<GetAllLabelsCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllLabelsCommandValidator"/> class.
        /// </summary>
        public GetAllLabelsCommandValidator()
        {
        }
    }

    /// <summary>
    /// Authorization for MediatR <see cref="GetAllLabelsCommand"/> Query.
    /// </summary>
    public class GetAllLabelsCommandAuthorization : IAuthorize<GetAllLabelsCommand, List<LabelViewModel>>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GetAllLabelsCommand, List<LabelViewModel>>.Authorize(GetAllLabelsCommand request)
        {
            bool authorized = true;

            if (request.User != null)
            {
                var claims = request.User.Claims;
                authorized = true;
            }

            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Handler class for the MediatR <see cref="GetAllQuestionTypesCommand"/> command.
    /// </summary>
    public class GetAllLabelsCommandHandler : IGetEntityCommandHandler<GetAllLabelsCommand, List<LabelViewModel>>
    {
        private readonly ILabelService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllLabelsCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILabelService "/> service.</param>
        public GetAllLabelsCommandHandler(ILabelService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<List<LabelViewModel>> Handle(GetAllLabelsCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GetAllLabels();
        }
    }
}
