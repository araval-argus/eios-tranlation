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
    /// ImportLabelsByLanguageAndGroupCommand MediatR Command.
    /// </summary>
    public class ImportLabelsByLanguageAndGroupCommand : Request<bool>, IGetEntityCommand<bool>
    {
        public int LabelGroupId { get; set; }
        public string LanguageCode { get; set; }
        public IFormFile File { get; set; }
    }

    /// <summary>
    /// Validation class for MediatR <see cref="ImportLabelsByLanguageAndGroupCommand"/>.
    /// </summary>
    public class ImportLabelsByLanguageAndGroupCommandValidator : AbstractValidator<ImportLabelsByLanguageAndGroupCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportLabelsByLanguageAndGroupCommandValidator"/> class.
        /// </summary>
        public ImportLabelsByLanguageAndGroupCommandValidator()
        {
            this.RuleFor(x => x.LabelGroupId).GreaterThan(0);
            this.RuleFor(x => x.LanguageCode).NotNull().NotEmpty();
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
    /// Authorization for MediatR <see cref="ImportLabelsByLanguageAndGroupCommand"/> Query.
    /// </summary>
    public class ImportLabelsByLanguageAndGroupCommandAuthorization : IAuthorize<ImportLabelsByLanguageAndGroupCommand, bool>
    {
        /// <inheritdoc/>
        async Task<bool> IAuthorize<ImportLabelsByLanguageAndGroupCommand, bool>.Authorize(ImportLabelsByLanguageAndGroupCommand request)
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
    public class ImportLabelsByLanguageAndGroupCommandHandler : IGetEntityCommandHandler<ImportLabelsByLanguageAndGroupCommand, bool>
    {
        private readonly ILabelService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportLabelsByLanguageAndGroupCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Instance of <see cref="ILabelService "/> service.</param>
        public ImportLabelsByLanguageAndGroupCommandHandler(ILabelService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public async Task<bool> Handle(ImportLabelsByLanguageAndGroupCommand request, CancellationToken cancellationToken)
        {
            return await this.service.ImportLabelsByLanguageAndGroup(request);
        }
    }
}
