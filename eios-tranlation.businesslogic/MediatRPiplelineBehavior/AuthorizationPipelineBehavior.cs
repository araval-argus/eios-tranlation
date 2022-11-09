

namespace eios_translation.businesslogic.MediatRPiplelineBehavior
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
    using MediatR;

    /// <summary>
    /// Interface Class to be Implemented by the MediatR Authorization Handlers.
    /// </summary>
    /// <typeparam name="TRequest">A Request <see cref="Request"/> instance. This would fetch the User's Claims.</typeparam>
    public interface IAuthorize<in TRequest, out TResponse>
        where TRequest : Request<TResponse>
    {
        /// <summary>
        /// The method that needs to be implemented.
        /// </summary>
        /// <param name="request">An instance of <see cref="Request"/> class.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<bool> Authorize(TRequest request);
    }

    /// <summary>
    /// Class for the MediatR Authorization pipeline behavior.
    /// Will track all the instances <see cref="Request"/> class which MediatR Command & Query object needs to derive from.
    /// </summary>
    /// <typeparam name="TRequest">TRequest.</typeparam>
    /// <typeparam name="TResponse">TResponse.</typeparam>
    public class AuthorizationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : Request<TResponse>
    {
        private readonly IAuthorize<TRequest, TResponse> authorize;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationPipelineBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="authorize">Instance of <see cref="IAuthorize"/> class.</param>
        public AuthorizationPipelineBehavior(IAuthorize<TRequest, TResponse> authorize)
        {
            this.authorize = authorize;
        }

        /// <inheritdoc/>
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
            )
        {
            if (!await this.authorize.Authorize(request))
            {
                return await Task.FromException<TResponse>(new UnauthorizedAccessException());
            }

            var response = await next();
            return response;
        }
    }
}
