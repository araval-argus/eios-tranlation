using eios_tranlation.businesslogic.Features.Label;
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

namespace eios_tranlation.businesslogic.Features.LabelGroup
{
    public class InsertLabelGroupCommand : Request<int>, ICreateUpdateCommand
    {
        public string GroupName { get; set; }
        public int? FK_ParentLableGroupId { get; set; }
    }

    public class InsertLabelGroupCommandAuthorization : IAuthorize<InsertLabelGroupCommand, int>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<InsertLabelGroupCommand, int>.Authorize(InsertLabelGroupCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }
    public class InsertLabelGroupCommandValidator : AbstractValidator<InsertLabelGroupCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateValidator"/> class.
        /// </summary>
        public InsertLabelGroupCommandValidator()
        {
            this.RuleFor(x => x.GroupName).NotEmpty().NotNull().MaximumLength(100);
        }
    }

    /// <summary>
    /// Handler class for the MediatR <see cref="Create"/> command.
    /// </summary>
    public class InsertLabelGroupCommandHandler : ICreateUpdateCommandHandler<InsertLabelGroupCommand>
    {
        private readonly ILabelGroupService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILabelGroupService"/> service.</param>
        public InsertLabelGroupCommandHandler(ILabelGroupService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<int> Handle(InsertLabelGroupCommand request, CancellationToken cancellationToken)
        {
            return await this.service.InsertLabelGroup(request);
        }
    }
}
