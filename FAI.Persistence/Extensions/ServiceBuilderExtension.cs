using FAI.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Persistence.Extensions
{
    public static class ServiceBuilderExtension
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            /* Scruter Extension .Scan */
            services.Scan(scan =>
            {
                scan.FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(c => c.WithAttribute<MapServiceDependencyAttribute>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
        }

        public static void RegisterRepositoriesMethod(IServiceCollection services)
        {
            /* Scruter Extension .Scan */
            services.Scan(scan =>
            {
                scan.FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(c => c.WithAttribute<MapServiceDependencyAttribute>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
        }

    }
}