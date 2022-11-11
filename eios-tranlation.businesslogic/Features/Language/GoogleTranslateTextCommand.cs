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
    /// GoogleTranslateText MediatR Command.
    /// </summary>
    public class GoogleTranslateTextCommand : Request<string>, IGetEntityCommand<string>
    {
        public string Source { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }

    }

    /// <summary>
    /// Validation class for MediatR <see cref="GoogleTranslateTextCommand"/>.
    /// </summary>
    public class GoogleTranslateTextCommandValidator : AbstractValidator<GoogleTranslateTextCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleTranslateTextCommandValidator"/> class.
        /// </summary>
        public GoogleTranslateTextCommandValidator()
        {
            this.RuleFor(x => x.Source).NotNull().NotEmpty();
            this.RuleFor(x => x.SourceLanguage).NotNull().NotEmpty();
            this.RuleFor(x => x.TargetLanguage).NotNull().NotEmpty();
        }
    }

    /// <summary>
    /// Authorization for MediatR <see cref="GoogleTranslateTextCommand"/> Query.
    /// </summary>
    public class GoogleTranslateTextCommandAuthorization : IAuthorize<GoogleTranslateTextCommand, string>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GoogleTranslateTextCommand, string>.Authorize(GoogleTranslateTextCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Handler class for the MediatR <see cref="GetAllQuestionTypesCommand"/> command.
    /// </summary>
    public class GoogleTranslateTextCommandHandler : IGetEntityCommandHandler<GoogleTranslateTextCommand, string>
    {
        private readonly ILanguageService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleTranslateTextCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILanguageService "/> service.</param>
        public GoogleTranslateTextCommandHandler(ILanguageService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<string> Handle(GoogleTranslateTextCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GoogleTranslate(request.Source, request.SourceLanguage, request.TargetLanguage);
        }
    }
}
