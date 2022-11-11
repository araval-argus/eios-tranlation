using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.MediatRPiplelineBehavior;
using eios_translation.core.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.businesslogic.Features.Language
{
    public class UpdateLanguageCommand : Request<LanguageViewModel>, IGetEntityCommand<LanguageViewModel>
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public float Tolerance { get; set; }
        public ToleranceType ToleranceType { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateLanguageCommandAuthorization : IAuthorize<UpdateLanguageCommand, LanguageViewModel>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<UpdateLanguageCommand, LanguageViewModel>.Authorize(UpdateLanguageCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    public class UpdateLanguageCommandValidator : AbstractValidator<UpdateLanguageCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateLanguageCommandValidator"/> class.
        /// </summary>
        public UpdateLanguageCommandValidator()
        {
            this.RuleFor(x => x.LanguageId).GreaterThan(0);
            this.RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
            this.RuleFor(x => x.LanguageCode).NotEmpty().NotNull().MaximumLength(10);
            this.RuleFor(x => x.ToleranceType).Must(this.CheckToleranceType).WithMessage("Invalid tolerance type is passed.");
        }
        private bool CheckToleranceType(ToleranceType tolType)
        {
            return tolType == ToleranceType.Absolute || tolType == ToleranceType.Relative;
        }
    }

    
    public class CreateChallengeCommandHandler : IGetEntityCommandHandler<UpdateLanguageCommand, LanguageViewModel>
    {
        private readonly ILanguageService service;
        public CreateChallengeCommandHandler(ILanguageService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<LanguageViewModel> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
        {
            return await this.service.UpdateLanguage(request);
        }
    }
}
