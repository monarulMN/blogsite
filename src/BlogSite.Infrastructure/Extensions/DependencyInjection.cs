using BlogSite.Application.Contracts;
using BlogSite.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependency(this IServiceCollection services)
        {
            // Add your infrastructure services here
            services.AddScoped<IApplicationUnitOfWork, ApplicationUnitOfWork>();

            return services;
        }
    }
}
