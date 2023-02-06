using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgDataCodingAssignment.Application.ServiceConfiguration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediator(options =>
            {
                options.ServiceLifetime = ServiceLifetime.Scoped;
                options.Namespace = "AgDataCodingAssignment.Application.Mediator";
            });

            //services.AddMediator(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
