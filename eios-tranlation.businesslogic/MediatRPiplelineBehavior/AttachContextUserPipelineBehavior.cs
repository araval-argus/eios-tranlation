

namespace  eios_tranlation.businesslogic.MediatRPiplelineBehavior
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// MediatR Pipeline behavior to add the current login user's context.
    /// </summary>
    /// <typeparam name="TRequest">TRequest.</typeparam>
    /// <typeparam name="TResponse">TResponse.</typeparam>
    public class AttachContextUserPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest,TResponse>
    where TRequest : Request<TResponse>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachContextUserPipelineBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HttpContext.</param>
        public AttachContextUserPipelineBehavior(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc/>
        public async Task<TResponse> Handle(TRequest request,  RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            request.User = null;
            if (this.httpContextAccessor.HttpContext.User != null && this.httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                request.User = this.httpContextAccessor.HttpContext.User;
            }

            var response = await next();
            return response;
        }
    }
}
