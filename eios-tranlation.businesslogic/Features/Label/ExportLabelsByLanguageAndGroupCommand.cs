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
    /// ExportLabelsByLanguageAndGroupCommand MediatR Command.
    /// </summary>
    public class ExportLabelsByLanguageAndGroupCommand : Request<string>, IGetEntityCommand<string>
    {
        public string LanguageCode { get; set; }

        public int LabelGroupId { get; set; }
    }

    /// <summary>
    /// Validation class for MediatR <see cref="ExportLabelsByLanguageAndGroupCommand"/>.
    /// </summary>
    public class ExportLabelsByLanguageAndGroupCommandValidator : AbstractValidator<ExportLabelsByLanguageAndGroupCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportLabelsByLanguageAndGroupCommandValidator"/> class.
        /// </summary>
        public ExportLabelsByLanguageAndGroupCommandValidator()
        {
            this.RuleFor(x => x.LanguageCode).NotNull().NotEmpty();
            this.RuleFor(x => x.LabelGroupId).GreaterThan(0);
        }
    }

    /// <summary>
    /// Authorization for MediatR <see cref="ExportLabelsByLanguageAndGroupCommand"/> Query.
    /// </summary>
    public class ExportLabelsByLanguageAndGroupCommandAuthorization : IAuthorize<ExportLabelsByLanguageAndGroupCommand, string>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<ExportLabelsByLanguageAndGroupCommand, string>.Authorize(ExportLabelsByLanguageAndGroupCommand request)
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
    public class ExportLabelsByLanguageAndGroupCommandHandler : IGetEntityCommandHandler<ExportLabelsByLanguageAndGroupCommand, string>
    {
        private readonly ILabelService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportLabelsByLanguageAndGroupCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILabelService "/> service.</param>
        public ExportLabelsByLanguageAndGroupCommandHandler(ILabelService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<string> Handle(ExportLabelsByLanguageAndGroupCommand request, CancellationToken cancellationToken)
        {
            return await this.service.ExportLabelsByLanguageAndGroup(request.LanguageCode, request.LabelGroupId);
        }
    }
}
