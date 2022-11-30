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
    /// ExportLabelsByLanguageIdCommand MediatR Command.
    /// </summary>
    public class ExportLabelsByLanguageIdCommand : Request<string>, IGetEntityCommand<string>
    {
        public string LanguageCode { get; set; }
    }

    /// <summary>
    /// Validation class for MediatR <see cref="ExportLabelsByLanguageIdCommand"/>.
    /// </summary>
    public class ExportLabelsByLanguageIdCommandValidator : AbstractValidator<ExportLabelsByLanguageIdCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportLabelsByLanguageIdCommandValidator"/> class.
        /// </summary>
        public ExportLabelsByLanguageIdCommandValidator()
        {
            this.RuleFor(x => x.LanguageCode).NotNull().NotEmpty();
        }
    }

    /// <summary>
    /// Authorization for MediatR <see cref="ExportLabelsByLanguageIdCommand"/> Query.
    /// </summary>
    public class ExportLabelsByLanguageIdCommandAuthorization : IAuthorize<ExportLabelsByLanguageIdCommand, string>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<ExportLabelsByLanguageIdCommand, string>.Authorize(ExportLabelsByLanguageIdCommand request)
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
    public class ExportLabelsByLanguageIdCommandHandler : IGetEntityCommandHandler<ExportLabelsByLanguageIdCommand, string>
    {
        private readonly ILabelService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportLabelsByLanguageIdCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILabelService "/> service.</param>
        public ExportLabelsByLanguageIdCommandHandler(ILabelService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<string> Handle(ExportLabelsByLanguageIdCommand request, CancellationToken cancellationToken)
        {
            return await this.service.ExportLabelsByLanguageId(request.LanguageCode);
        }
    }
}
