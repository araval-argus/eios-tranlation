namespace eios_tranlation.businesslogic.Features.Label
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
    /// ExportLabelsByLanguageIdCommand MediatR Command.
    /// </summary>
    public class ExportLabelsByGroupIdCommand : Request<string>, IGetEntityCommand<string>
    {
        public int LanguageId { get; set; }
        public int GroupId { get; set; }
    }
    /// <summary>
    /// Validation class for MediatR <see cref="ExportLabelsByLanguageIdCommand"/>.
    /// </summary>
    public class ExportLabelsByGroupIdCommandValidator : AbstractValidator<ExportLabelsByLanguageIdCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportLabelsByLanguageIdCommandValidator"/> class.
        /// </summary>
        public ExportLabelsByGroupIdCommandValidator()
        {
        }
    }
    /// <summary>
    /// Authorization for MediatR <see cref="ExportLabelsByLanguageIdCommand"/> Query.
    /// </summary>
    public class ExportLabelsByGroupIdCommandAuthorization : IAuthorize<ExportLabelsByGroupIdCommand, string>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<ExportLabelsByGroupIdCommand, string>.Authorize(ExportLabelsByGroupIdCommand request)
        {
            bool authorized = true;

            if (request.User != null)
            {
                var claims = request.User.Claims;
                authorized = true;
            }

            return await Task.FromResult(authorized);
        }
    }
    /// <summary>
    /// Handler class for the MediatR <see cref="ExportLabelsByGroupIdCommand"/> command.
    /// </summary>
    public class ExportLabelsByGroupIdCommandHandler : IGetEntityCommandHandler<ExportLabelsByGroupIdCommand, string>
    {
        private readonly ILabelService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportLabelsByLanguageIdCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILabelService "/> service.</param>
        public ExportLabelsByGroupIdCommandHandler(ILabelService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<string> Handle(ExportLabelsByGroupIdCommand request, CancellationToken cancellationToken)
        {
            return await this.service.ExportLabelsByGroupId(request.LanguageId,request.GroupId);
        }
    }
}
