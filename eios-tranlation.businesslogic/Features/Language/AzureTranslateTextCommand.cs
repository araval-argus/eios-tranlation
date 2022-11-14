namespace eios_tranlation.businesslogic.Features.Language
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
    /// GetAllLanguages MediatR Command.
    /// </summary>
    public class AzureTranslateTextCommand : Request<string>, IGetEntityCommand<string>
    {
        public string Source { get; set; }
        public string TargetLanguage { get; set; }
        public string SourceLanguage { get; set; }

    }

    /// <summary>
    /// Validation class for MediatR <see cref="AzureTranslateTextCommand"/>.
    /// </summary>
    public class AzureTranslateTextCommandValidator : AbstractValidator<AzureTranslateTextCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTranslateTextCommandValidator"/> class.
        /// </summary>
        public AzureTranslateTextCommandValidator()
        {
            this.RuleFor(x => x.Source).NotEmpty().NotNull();
            this.RuleFor(x => x.TargetLanguage).NotEmpty().NotNull();
            this.RuleFor(x => x.SourceLanguage).NotEmpty().NotNull();
        }
    }

    /// <summary>
    /// Authorization for MediatR <see cref="AzureTranslateTextCommand"/> Query.
    /// </summary>
    public class AzureTranslateTextCommandAuthorization : IAuthorize<AzureTranslateTextCommand, string>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<AzureTranslateTextCommand, string>.Authorize(AzureTranslateTextCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Handler class for the MediatR <see cref="GetAllQuestionTypesCommand"/> command.
    /// </summary>
    public class AzureTranslateTextCommandHandler : IGetEntityCommandHandler<AzureTranslateTextCommand, string>
    {
        private readonly ILanguageService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTranslateTextCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILanguageService "/> service.</param>
        public AzureTranslateTextCommandHandler(ILanguageService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<string> Handle(AzureTranslateTextCommand request, CancellationToken cancellationToken)
        {
            return await this.service.AzureTranslate(request.Source, request.SourceLanguage, request.TargetLanguage);
        }
    }
}
