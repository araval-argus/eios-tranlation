using eios_tranlation.businesslogic.Features.Language;
using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
using eios_tranlation.businesslogic.ServiceInterfaces;
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
    public class InsertLabelCommand : Request<int>, ICreateUpdateCommand
    {
        public string ResourceId { get; set; }
        public int FK_LabelGroupId { get; set; }
        public int FK_LanguageId { get; set; }
        public string LabelValue { get; set; }
        public LabelType LabelType { get; set; }
        public string? LabelDescription { get; set; }
        public string? LabelSnapshotPath { get; set; }
    }

    public class InsertLabelCommandAuthorization : IAuthorize<InsertLabelCommand, int>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<InsertLabelCommand, int>.Authorize(InsertLabelCommand request)
        {
            bool authorized = true;
            return await Task.FromResult(authorized);
        }
    }
    public class InsertLabelCommandValidator : AbstractValidator<InsertLabelCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateValidator"/> class.
        /// </summary>
        public InsertLabelCommandValidator()
        {
            this.RuleFor(x => x.LabelValue).NotEmpty().NotNull().MaximumLength(100);
        }
    }

    /// <summary>
    /// Handler class for the MediatR <see cref="Create"/> command.
    /// </summary>
    public class InsertLabelCommandHandler : ICreateUpdateCommandHandler<InsertLabelCommand>
    {
        private readonly ILabelService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILabelService"/> service.</param>
        public InsertLabelCommandHandler(ILabelService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<int> Handle(InsertLabelCommand request, CancellationToken cancellationToken)
        {
            return await this.service.InsertLabel(request);
        }
    }
}
