

namespace  eios_tranlation.businesslogic.MediatRPiplelineBehavior
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using eios_translation.core.Wrappers;
    using FluentValidation;
    using MediatR;

    /// <summary>
    /// Class for the MediatR Validation Pipeline.
    /// </summary>
    /// <typeparam name="TRequest">TRequest.</typeparam>
    /// <typeparam name="TResponse">TResponse.</typeparam>
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : Request<TResponse>
    {
        private readonly List<IValidator<TRequest>> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationPipelineBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="validators">List of All Validators to verify.</param>
        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators.ToList();
        }

        /// <inheritdoc/>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = this.validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                return await Task.FromException<TResponse>(new ApiException("Validation Errors", 500, failures.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage))));
            }

            return await next();
        }
    }
}
