using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;

namespace Client.Services
{
    public static class ServiceExtension
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class ScopedServiceAttribute : Attribute { }

        [AttributeUsage(AttributeTargets.Class)]
        public class SingletonServiceAttribute : Attribute { }

        [AttributeUsage(AttributeTargets.Class)]
        public class TransistentServiceAttribute : Attribute { }

        public static IServiceCollection RegisterServicesFromAttributes(this IServiceCollection servicesCollection)
        {
            Type scopedServiceType = typeof(ScopedServiceAttribute);
            Type singletonServiceType = typeof(SingletonServiceAttribute);
            Type transistentServiceType = typeof(TransistentServiceAttribute);

            var services = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsDefined(scopedServiceType, true)
                || type.IsDefined(singletonServiceType, true)
                || type.IsDefined(transistentServiceType, true)
                && !type.IsInterface)
                .Select(type => new
                {
                    Interface = type.GetInterface($"I{type.Name}"),
                    Implementation = type
                });

            var servicesWithInterfaces = services.Where(service => service.Interface is not null);
            var servicesWithoutInterfaces = services.Where(service => service.Interface is null);

            foreach (var service in servicesWithInterfaces)
            {
                if (service.Implementation.IsDefined(scopedServiceType, false))
                {
                    servicesCollection.AddScoped(service.Interface!, service.Implementation);
                }

                if (service.Implementation.IsDefined(singletonServiceType, false))
                {
                    servicesCollection.AddSingleton(service.Interface!, service.Implementation);
                }

                if (service.Implementation.IsDefined(transistentServiceType, false))
                {
                    servicesCollection.AddTransient(service.Interface!, service.Implementation);
                }
            }

            foreach (var service in servicesWithoutInterfaces)
            {
                if (service.Implementation.IsDefined(scopedServiceType, false))
                {
                    servicesCollection.AddScoped(service.Implementation);
                }

                if (service.Implementation.IsDefined(singletonServiceType, false))
                {
                    servicesCollection.AddSingleton(service.Implementation);
                }

                if (service.Implementation.IsDefined(transistentServiceType, false))
                {
                    servicesCollection.AddTransient(service.Implementation);
                }
            }

            return servicesCollection;
        }
    }
}
