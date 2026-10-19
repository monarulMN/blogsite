using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Domain.Contracts;
using BlogSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Contracts.Repositories
{
    public interface IBlogPostRepository : IRepository<BlogPost, Guid>
    {
        Task<(IList<BlogPost>, int, int)> GetPagedAdminBlogPostsAsync(int pageSize, 
            int pageIndex, 
            string? sortOrder, 
            string? searchText, 
            CancellationToken cancellationToken);

        Task<(IList<BlogPost>, int, int)> GetPagedBlogPostsAsync(GetAllBlogPostsQuery query, 
            CancellationToken cancellationToken);
    }
}
