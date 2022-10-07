using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eios_translation.businesslogic.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAllTypesWithBaseInterface<T>(this IServiceCollection services, Assembly[] assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
            foreach (var type in typesFromAssemblies)
            {
                Type myType = type.GetInterfaces()[0];
                services.Add(new ServiceDescriptor(myType, type, lifetime));
            }
        }
    }
}
