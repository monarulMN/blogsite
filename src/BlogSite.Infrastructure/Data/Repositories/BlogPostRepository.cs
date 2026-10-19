using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Repositories
{
    public class BlogPostRepository : Repository<BlogPost, Guid>, IBlogPostRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public BlogPostRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _applicationDbContext = dbContext;
        }

        public async Task<(IList<BlogPost>, int, int)> GetPagedAdminBlogPostsAsync(int pageSize,
            int pageIndex, 
            string? sortOrder, 
            string? searchText, 
            CancellationToken cancellationToken)
        {
            return await GetDynamicAsync(pageIndex: pageIndex, 
            pageSize: pageSize, 
            orderBy: sortOrder, 
            filter: x => 
                x.Title.Contains(searchText) &&
                x.Status == Status.Active, 
            include: x => 
                x.Include(b => b.Blogsite));
        }

        public async Task<(IList<BlogPost>, int, int)> GetPagedBlogPostsAsync(GetBlogPostsQuery query, 
            CancellationToken cancellationToken)
        {
            return await GetDynamicAsync(x =>
                (
                    query.SearchText == null ||
                    x.Title.Contains(query.SearchText)
                 ) && x.Status == Status.Active
                   && x.BlogSiteId == query.BlogSiteId,
                 query.SortOrder,
                 null,
                 query.PageIndex,
                 query.PageSize,
                 true,
                 cancellationToken);

        }
    }
}
