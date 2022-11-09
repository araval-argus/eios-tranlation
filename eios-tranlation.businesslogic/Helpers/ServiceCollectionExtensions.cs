namespace eios_tranlation.businesslogic.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using eios_tranlation.businesslogic.MediatRPiplelineBehavior;
    using eios_translation.businesslogic.MediatRPiplelineBehavior;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> class.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method to add the MediatR Pipleline handlers.
        /// </summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/> service.</param>
        public static void AddMediatRPipelines(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AttachContextUserPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        }

        /// <summary>
        /// Extension Method to add all the handler for a particular generic Type T Interface.
        /// </summary>
        /// <typeparam name="T">Generic Type.</typeparam>
        /// <param name="services">Instance of <see cref="IServiceCollection"/> service.</param>
        /// <param name="assemblies">List of Assemblies to Scan for the Interface.</param>
        /// <param name="lifetime">Lifespan for the Service.</param>
        public static void RegisterAllTypesWithBaseInterface<T>(this IServiceCollection services, Assembly[] assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
            foreach (var type in typesFromAssemblies)
            {
                Type myType = type.GetInterfaces()[0];
                services.Add(new ServiceDescriptor(myType, type, lifetime));
            }
        }

        /// <summary>
        /// Extension method to add all the MediatR Authorization Handlers for the Authorization Pipeline behavior.
        /// </summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/> service.</param>
        /// <param name="assemblies">List of Assemblies to Scan for the Interface.</param>
        /// <param name="lifetime">Lifespan for the Service.</param>
        public static void RegisterAuthorizationHandlers(this IServiceCollection services, Assembly[] assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            // Could have more robust solution than hard-coding string. But will do for now.
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterface("IAuthorize`2") != null));
            foreach (var type in typesFromAssemblies)
            {
                Type myType = type.GetInterface("IAuthorize`2");
                services.Add(new ServiceDescriptor(myType, type, lifetime));
            }
        }
    }
}
