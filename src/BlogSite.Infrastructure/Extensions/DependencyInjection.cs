using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Infrastructure.Data;
using BlogSite.Infrastructure.Data.Repositories;
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
            services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<ISqlUtility, SqlUtility>();
            services.AddScoped<IBlogSiteRepository, BlogSiteRepository>();

            services.AddSingleton<IServerTime, ServerTime>();

            return services;
        }
    }
}
