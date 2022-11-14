using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.MediatRPiplelineBehavior;
using eios_translation.businesslogic.ServiceInterfaces;
using FluentValidation;
using eios_translation.core.Common;

namespace eios_tranlation.businesslogic.Features.Language
{
    public class InsertLanguageCommand : Request<int>, ICreateUpdateCommand
    {
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public float Tolerance { get; set; }
        public ToleranceType ToleranceType { get; set; }
        public string? Description { get; set; }
    }


    public class InsertLanguageCommandAuthorization : IAuthorize<InsertLanguageCommand, int>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<InsertLanguageCommand, int>.Authorize(InsertLanguageCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }


    public class InsertLanguageCommandValidator : AbstractValidator<InsertLanguageCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateValidator"/> class.
        /// </summary>
        public InsertLanguageCommandValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
            this.RuleFor(x => x.LanguageCode).NotEmpty().NotNull().MaximumLength(10);
            //this.RuleFor(x => x.ToleranceType).Must(this.CheckToleranceType).WithMessage("Invalid tolerance type is passed.");
        }
        private bool CheckToleranceType(ToleranceType tolType)
        {
            return tolType == ToleranceType.Absolute || tolType == ToleranceType.Relative;
        }
    }

    /// <summary>
    /// Handler class for the MediatR <see cref="Create"/> command.
    /// </summary>
    public class InsertLanguageCommandHandler : ICreateUpdateCommandHandler<InsertLanguageCommand>
    {
        private readonly ILanguageService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILanguageService"/> service.</param>
        public InsertLanguageCommandHandler(ILanguageService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<int> Handle(InsertLanguageCommand request, CancellationToken cancellationToken)
        {
            return await this.service.InsertLanguage(request);
        }
    }
}
