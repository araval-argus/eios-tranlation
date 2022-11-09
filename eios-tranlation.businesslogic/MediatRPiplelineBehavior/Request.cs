
namespace  eios_tranlation.businesslogic.MediatRPiplelineBehavior
{
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    /// <summary>
    /// The Base class for all MediatR Command/Queries. Required for Authorization Pipeline behavior & User Context behavior.
    /// The User Context Behavior will fill in this using the current HttpContext.
    /// Then the Authorization behavior will check this against a Authorization Policy.
    /// </summary>
    public class Request<TResponse> : IRequest<TResponse>
    {
        /// <summary>
        /// Gets or sets the current login user.
        /// </summary>
        [JsonIgnore]
        public ClaimsPrincipal User { get; set; }
    }
}
