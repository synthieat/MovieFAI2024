using FAI.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Application.Extensions
{
 
    public static class ServiceBuilderExtension
    {
        public static void RegisterServices(this IServiceCollection services)
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
