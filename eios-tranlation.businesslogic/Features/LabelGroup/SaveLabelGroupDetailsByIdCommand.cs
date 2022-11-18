using eios_tranlation.businesslogic.Features.LabelGroup;
using eios_tranlation.businesslogic.Features.LabelGroup.Validators;
using eios_tranlation.businesslogic.Features.LabelGroup.ViewModels;
using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.MediatRPiplelineBehavior;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.businesslogic.Features.LabelGroup
{
    public class SaveLabelGroupDetailsByIdCommand : Request<LabelGroupDetailViewModel>, IGetEntityCommand<LabelGroupDetailViewModel>
    {
        public int LabelGroupId { get; set; }
        public List<LabelWithLanguage> Labels { get; set; } = new List<LabelWithLanguage>();
    }
    /// <summary>
    /// Validation class for MediatR <see cref="SaveLabelGroupDetailsByIdCommand"/>.
    /// </summary>
    public class SaveLabelGroupDetailsByIdCommandValidator : AbstractValidator<SaveLabelGroupDetailsByIdCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveLabelGroupDetailsByIdCommandValidator"/> class.
        /// </summary>
        public SaveLabelGroupDetailsByIdCommandValidator()
        {
            this.RuleFor(x=>x.LabelGroupId).GreaterThan(0);
            this.RuleForEach(x => x.Labels).SetValidator(new LanguageLabelsValidator());

        }
    }
    /// <summary>
    /// Authorization for MediatR <see cref="SaveLabelGroupDetailsByIdCommand"/> Query.
    /// </summary>
    public class SaveLabelGroupDetailsByIdCommandAuthorization : IAuthorize<SaveLabelGroupDetailsByIdCommand, LabelGroupDetailViewModel>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<SaveLabelGroupDetailsByIdCommand, LabelGroupDetailViewModel>.Authorize(SaveLabelGroupDetailsByIdCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    /// <summary>
    /// Handler class for the MediatR <see cref="GetAllQuestionTypesCommand"/> command.
    /// </summary>
    public class SaveLabelGroupDetailsByIdCommandHandler : IGetEntityCommandHandler<SaveLabelGroupDetailsByIdCommand, LabelGroupDetailViewModel>
    {
        private readonly ILabelGroupService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveLabelGroupDetailsByIdCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILabelGroupService "/> service.</param>
        public SaveLabelGroupDetailsByIdCommandHandler(ILabelGroupService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<LabelGroupDetailViewModel> Handle(SaveLabelGroupDetailsByIdCommand request, CancellationToken cancellationToken)
        {
            return await this.service.SaveLabelGroupDetailsById(request);
        }
    }
}
