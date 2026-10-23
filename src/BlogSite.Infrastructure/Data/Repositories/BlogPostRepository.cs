using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Domain.Dtos;
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

        public async Task<(IList<BlogPost>, int, int)> GetPagedBlogPostsAsync(GetAllBlogPostsQuery query, 
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

        public async Task<PublicBlogPostDto> GetPublicBlogPostWithTagAsync(Guid blogPostId)
        {
            return await _applicationDbContext.BlogPosts
                .AsNoTracking()
                .Where(x =>
                    x.Id == blogPostId &&
                    x.Status == Status.Active &&
                    x.PostStatus == BlogStatus.Publish)
                .Select(x => new PublicBlogPostDto
                {
                    Id = x.Id,
                    Author = x.Blogsite.Name,
                    AuthorSlag = x.Blogsite.Slug,
                    Title = x.Title,
                    Body = x.Body,
                    ThumbnailPath = x.ThumbnailPath,
                    CreatedAt = x.CreatedAt,

                    Tags = x.BlogPostTags
                        .Where(pt =>
                            pt.Status == Status.Active &&
                            pt.Tag.Status == Status.Active)
                        .Select(pt => pt.Tag)
                        .ToList()

                    //Comments = x.Comments
                    //    .Where(c => c.Status == Status.Active)
                    //    .OrderByDescending(c => c.CreateAt)
                    //    .ToList()
                })
                .FirstOrDefaultAsync();
                        
        }
    }
}
