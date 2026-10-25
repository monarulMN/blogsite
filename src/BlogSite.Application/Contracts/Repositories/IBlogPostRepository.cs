using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Domain.Contracts;
using BlogSite.Domain.Dtos;
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

        Task<(IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)> GetPublicBlogPostAsync(int pageIndex, 
            int pageSize, 
            CancellationToken cancellationToken);

        Task<(IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)> GetPublicBlogPostAsync(int pageIndex, 
            int pageSize, 
            Guid blogSiteId, 
            CancellationToken cancellationToken);

        Task<(IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)> 
            GetPublicBlogPostsByBlogSiteAndCategoryAsync(int pageIndex, 
                int pageSize, 
                Guid blogSiteId, 
                Guid categoryId, 
                CancellationToken cancellationToken);

        Task<(IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)> 
            GetPublicBlogPostsByBlogSiteAndTagAsync(int pageIndex, 
                int pageSize, 
                Guid blogSiteId, 
                Guid tagId, 
                CancellationToken cancellationToken);

        Task<(IReadOnlyList<PublicBlogPostDto> data, bool HasMore)> GetPublicBlogPostsByTagAsync(int pageIndex, 
            int pageSize, 
            Guid tagId, 
            CancellationToken cancellationToken);

        Task<PublicBlogPostDto> GetPublicBlogPostWithTagAsync(Guid blogPostId);

        Task<BlogPost> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
