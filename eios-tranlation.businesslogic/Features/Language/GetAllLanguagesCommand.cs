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
    public class GetAllLanguagesCommand : Request<List<LanguageViewModel>>, IGetEntityCommand<List<LanguageViewModel>>
    {
        public int LanguageId { get; set; }
    }

    /// <summary>
    /// Validation class for MediatR <see cref="GetAllLanguagesCommand"/>.
    /// </summary>
    public class GetAllLanguagesCommandValidator : AbstractValidator<GetAllLanguagesCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllLanguagesCommandValidator"/> class.
        /// </summary>
        public GetAllLanguagesCommandValidator()
        {
        }
    }

    /// <summary>
    /// Authorization for MediatR <see cref="GetAllLanguagesCommand"/> Query.
    /// </summary>
    public class GetAllLanguagesCommandAuthorization : IAuthorize<GetAllLanguagesCommand, List<LanguageViewModel>>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GetAllLanguagesCommand, List<LanguageViewModel>>.Authorize(GetAllLanguagesCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Handler class for the MediatR <see cref="GetAllQuestionTypesCommand"/> command.
    /// </summary>
    public class GetAllLanguagesCommandHandler : IGetEntityCommandHandler<GetAllLanguagesCommand, List<LanguageViewModel>>
    {
        private readonly ILanguageService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllLanguagesCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILanguageService "/> service.</param>
        public GetAllLanguagesCommandHandler(ILanguageService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<List<LanguageViewModel>> Handle(GetAllLanguagesCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GetAllLanguages();
        }
    }
}
