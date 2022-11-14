using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
using eios_translation.businesslogic.Features.Label.ViewModels;
using eios_translation.businesslogic.MediatRPiplelineBehavior;
using eios_translation.core.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eios_tranlation.businesslogic.Features.LabelGroup
{
    public class UpdateLabelGroupCommand : Request<LabelGroupViewModel>, IGetEntityCommand<LabelGroupViewModel>
    {
        public int LabelGroupId { get; set; }
        public string GroupName { get; set; }
        public int? FK_ParentLableGroupId { get; set; }
    }
    public class UpdateLabelGroupCommandAuthorization : IAuthorize<UpdateLabelGroupCommand, LabelGroupViewModel>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<UpdateLabelGroupCommand, LabelGroupViewModel>.Authorize(UpdateLabelGroupCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }

    public class UpdateLabelGroupCommandValidator : AbstractValidator<UpdateLabelGroupCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateLabelGroupCommandValidator"/> class.
        /// </summary>
        public UpdateLabelGroupCommandValidator()
        {
            this.RuleFor(x => x.LabelGroupId).GreaterThan(0);
            this.RuleFor(x => x.GroupName).NotEmpty().NotNull().MaximumLength(100);
            this.RuleFor(x => x.FK_ParentLableGroupId).GreaterThan(0).When(x => x.FK_ParentLableGroupId != null);
        }

    }
    public class UpdateLabelGroupCommandHandler : IGetEntityCommandHandler<UpdateLabelGroupCommand, LabelGroupViewModel>
    {
        private readonly ILabelGroupService service;
        public UpdateLabelGroupCommandHandler(ILabelGroupService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<LabelGroupViewModel> Handle(UpdateLabelGroupCommand request, CancellationToken cancellationToken)
        {
            return await this.service.UpdateLabelGroup(request);
        }
    }

}
