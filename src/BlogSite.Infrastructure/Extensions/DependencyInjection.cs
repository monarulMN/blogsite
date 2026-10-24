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
            services.AddScoped<IBlogSiteRepository, BlogSiteRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            services.AddSingleton<IServerTime, ServerTime>();

            return services;
        }
    }
}
