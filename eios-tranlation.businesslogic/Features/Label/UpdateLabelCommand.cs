using eios_tranlation.businesslogic.Features.Language;
using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.MediatRPiplelineBehavior;
using eios_translation.businesslogic.ServiceInterfaces;
using eios_translation.core.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.businesslogic.Features.Label
{
    public class UpdateLabelCommand : Request<LabelViewModel>, IGetEntityCommand<LabelViewModel>
    {
        public int LabelId { get; set; }
        public string ResourceId { get; set; }
        public int FK_LabelGroupId { get; set; }
        public int FK_LanguageId { get; set; }
        public string LabelValue { get; set; }
        public LabelType LabelType { get; set; }
        public string? LabelDescription { get; set; }
        public string? LabelSnapshotPath { get; set; }
        public string? MachineTranslation { get; set; }
        public TranslationStatus TranslationStatus { get; set; }
        public string? Scope { get; set; }
        public int Version { get; set; }
        public bool IsActive { get; set; }
        public int? FK_PrevVersionLabelId { get; set; }
    }
    public class UpdateLabelCommandAuthorization : IAuthorize<UpdateLabelCommand, LabelViewModel>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<UpdateLabelCommand, LabelViewModel>.Authorize(UpdateLabelCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    public class UpdateLanguageCommandValidator : AbstractValidator<UpdateLabelCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateLanguageCommandValidator"/> class.
        /// </summary>
        public UpdateLanguageCommandValidator()
        {
            this.RuleFor(x => x.LabelId).GreaterThan(0);
            this.RuleFor(x => x.LabelValue).NotEmpty().NotNull().MaximumLength(100);
        }
    }


    public class UpdateLabelCommandHandler : IGetEntityCommandHandler<UpdateLabelCommand, int>
    {
        private readonly ILabelService service;
        public UpdateLabelCommandHandler(ILabelService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<int> Handle(UpdateLabelCommand request, CancellationToken cancellationToken)
        {
            return await this.service.UpdateLabel(request);
        }
    }
}
