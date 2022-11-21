namespace eios_tranlation.businesslogic.Features.Label
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
    using eios_tranlation.businesslogic.ServiceInterfaces;
    using eios_tranlation.core.Constants;
    using eios_translation.businesslogic.Features.Label.ViewModels;
    using eios_translation.businesslogic.MediatRPiplelineBehavior;
    using eios_translation.businesslogic.ServiceInterfaces;
    using FluentValidation;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// ImportLabelsByLanguageIdCommand MediatR Command.
    /// </summary>
    public class ImportLabelsByLanguageIdCommand : Request<bool>, IGetEntityCommand<bool>
    {
        public int LanguageId { get; set; }
        public IFormFile File{ get; set; }
    }

    /// <summary>
    /// Validation class for MediatR <see cref="ImportLabelsByLanguageIdCommand"/>.
    /// </summary>
    public class ImportLabelsByLanguageIdCommandValidator : AbstractValidator<ImportLabelsByLanguageIdCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportLabelsByLanguageIdCommandValidator"/> class.
        /// </summary>
        public ImportLabelsByLanguageIdCommandValidator()
        {
            this.RuleFor(x => x.LanguageId).GreaterThan(0);
            this.RuleFor(x => x.File).NotNull().WithMessage("No file found. File must not be empty");
            this.RuleFor(x => x.File).Custom((file, context) =>
            {
                if (file == null)
                {
                    return;
                }

                var ext = Path.GetExtension(file.FileName).Replace(".", string.Empty).ToLower();
                if (ext != "json")
                {
                    context.AddFailure("Invalid file type. Please upload valid file.");
                }

                if (file.Length > CommonSettings.AppSettings.MaxImportFileLength)
                {
                    context.AddFailure("File can not be larger than " + CommonSettings.AppSettings.MaxImportFileLength.ToString() + " bytes.");
                }
            });
        }
    }

    /// <summary>
    /// Authorization for MediatR <see cref="ImportLabelsByLanguageIdCommand"/> Query.
    /// </summary>
    public class ImportLabelsByLanguageIdCommandAuthorization : IAuthorize<ImportLabelsByLanguageIdCommand, bool>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<ImportLabelsByLanguageIdCommand, bool>.Authorize(ImportLabelsByLanguageIdCommand request)
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
    /// Handler class for the MediatR <see cref="GetAllQuestionTypesCommand"/> command.
    /// </summary>
    public class ImportLabelsByLanguageIdCommandHandler : IGetEntityCommandHandler<ImportLabelsByLanguageIdCommand, bool>
    {
        private readonly ILabelService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportLabelsByLanguageIdCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILabelService "/> service.</param>
        public ImportLabelsByLanguageIdCommandHandler(ILabelService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<bool> Handle(ImportLabelsByLanguageIdCommand request, CancellationToken cancellationToken)
        {
            return await this.service.ImportLabelsByLanguageId(request);
        }
    }
}
