using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.MediatRPiplelineBehavior;
using eios_translation.businesslogic.ServiceInterfaces;
using FluentValidation;

namespace eios_tranlation.businesslogic.Features.Language
{
    public class GetSelectedLanguagesCommand : Request<LanguageViewModel>, IGetEntityCommand<LanguageViewModel>
    {
        public int LanguageId { get; set; }
    }
    /// <summary>
    /// Validation class for MediatR <see cref="GetAllLanguagesCommand"/>.
    /// </summary>
    public class GetSelectedLanguagesCommandValidator : AbstractValidator<GetSelectedLanguagesCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllLanguagesCommandValidator"/> class.
        /// </summary>
        public GetSelectedLanguagesCommandValidator()
        {
            this.RuleFor(x => x.LanguageId).GreaterThan(0);
        }
    }
    /// <summary>
    /// Authorization for MediatR <see cref="GetAllLanguagesCommand"/> Query.
    /// </summary>
    public class GetSelectedLanguagesCommandAuthorization : IAuthorize<GetSelectedLanguagesCommand, LanguageViewModel>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<GetSelectedLanguagesCommand, LanguageViewModel>.Authorize(GetSelectedLanguagesCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Handler class for the MediatR <see cref="GetAllQuestionTypesCommand"/> command.
    /// </summary>
    public class GetSelectLanguagesCommandHandler : IGetEntityCommandHandler<GetSelectedLanguagesCommand, LanguageViewModel>
    {
        private readonly ILanguageService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllLanguagesCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILanguageService "/> service.</param>
        public GetSelectLanguagesCommandHandler(ILanguageService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<LanguageViewModel> Handle(GetSelectedLanguagesCommand request, CancellationToken cancellationToken)
        {
            return await this.service.GetSelectedLanguage(request.LanguageId);
        }
    }
}
