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

        //public async Task<BlogPost> GetByIdAsync(Guid id)
        //{
        //    return await GetByIdAsync(id,
        //        x => x
        //            .Include(b => b.BlogSite) 
        //            .Include(b => b.BlogPostCategories
        //                .Where(pc => pc.Category.Status == Status.Active))
        //                .ThenInclude(pc => pc.Category) 
        //            .Include(b => b.BlogPostTags
        //                .Where(pt => pt.Tag.Status == Status.Active))
        //                .ThenInclude(pt => pt.Tag));
        //}

        public async Task<(IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)> GetPublicBlogPostAsync(int pageIndex, 
            int pageSize, CancellationToken cancellationToken)
        {
            return await _applicationDbContext.BlogPosts
                .Where(x =>
                    x.Status == Status.Active &&
                    x.PostStatus == BlogStatus.Publish)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize + 1)
                .Select(x =>
                    new PublicBlogPostDto
                    {
                        Id = x.Id,
                        Author = x.Blogsite.Name,
                        AuthorSlag = x.Blogsite.Slug,
                        Title = x.Title,
                        Body = x.Body,
                        ThumbnailPath = x.ThumbnailPath,
                        CreatedAt = x.CreatedAt,
                        Tags = x.BlogPostTags.Select(pt => pt.Tag).ToList()
                    })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync()
                .ContinueWith(t =>
                {
                    var data = t.Result.Take(pageSize).ToList();
                    var hasMore = t.Result.Count > pageSize;
                    return (data, hasMore);
                });
        }

        public async Task<(IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)> GetPublicBlogPostAsync(int pageIndex, 
            int pageSize, 
            Guid blogSiteId, 
            CancellationToken cancellationToken)
        {
            return await _applicationDbContext.BlogPosts
                .Where(x => 
                    x.Status == Status.Active &&
                    x.PostStatus == BlogStatus.Publish &&
                    x.BlogSiteId == blogSiteId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize + 1)
                .Select(x => new PublicBlogPostDto
                {
                    Id = x.Id,
                    Author = x.Blogsite.Name,
                    AuthorSlag = x.Blogsite.Slug,
                    Title = x.Title,
                    Body = x.Body,
                    ThumbnailPath = x.ThumbnailPath,
                    CreatedAt = x.CreatedAt,
                    Tags = x.BlogPostTags.Select(pt => pt.Tag).ToList()
                }).OrderByDescending(x => x.CreatedAt)
                .ToListAsync()
                .ContinueWith(t =>
                {
                    var data = t.Result.Take(pageSize).ToList();
                    var hasMore = t.Result.Count > pageSize;
                    return (data, hasMore);
                });
        }

        public async Task<(IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)> 
            GetPublicBlogPostsByBlogSiteAndCategoryAsync(int pageIndex, 
                int pageSize, 
                Guid blogSiteId, 
                Guid categoryId, 
                CancellationToken cancellationToken)
        {
            var query = _applicationDbContext.BlogPosts
                .AsNoTracking()
                .Where(x =>
                    x.Status == Status.Active &&
                    x.PostStatus == BlogStatus.Publish &&
                    x.BlogSiteId == blogSiteId &&
                    x.BlogPostCategories.Any(y =>
                        y.CategoryId == categoryId &&
                        y.Status == Status.Active &&
                        y.Category.Status == Status.Active))
                .OrderByDescending(x => x.CreatedAt);

            var result = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize + 1)
                .Select(x =>
                    new PublicBlogPostDto
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
                                pt.Status == Status.Active)
                            .Select(pt => pt.Tag)
                            .ToList()
                    })
                .ToListAsync();

            var hasMore = result.Count > pageSize;
            var data = result.Take(pageSize).ToList();

            return (data, hasMore);
        }

        public async Task<(IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)> 
            GetPublicBlogPostsByBlogSiteAndTagAsync(int pageIndex, 
                int pageSize, 
                Guid blogSiteId, 
                Guid tagId, 
                CancellationToken cancellationToken)
        {
            var query = _applicationDbContext.BlogPosts
                .AsNoTracking()
                .Where(x =>
                    x.Status == Status.Active &&
                    x.PostStatus == BlogStatus.Publish &&
                    x.BlogSiteId == blogSiteId &&
                    x.BlogPostTags.Any(y =>
                        y.TagId == tagId &&
                        y.Status == Status.Active &&
                        y.Tag.Status == Status.Active
                    ))
                .OrderByDescending(x => x.CreatedAt);

            var result = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize + 1)
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
                        .Where(pt => pt.Status == Status.Active)
                        .Select(pt => pt.Tag)
                        .ToList()
                })
                .ToListAsync();

            var hasMore = result.Count > pageSize;
            var data = result.Take(pageSize).ToList();

            return (data, hasMore);
        }

        public async Task<(IReadOnlyList<PublicBlogPostDto> data, bool HasMore)> GetPublicBlogPostsByTagAsync(
            int pageIndex, 
            int pageSize, 
            Guid tagId, 
            CancellationToken cancellationToken)
        {
            var query = _applicationDbContext.BlogPosts
                .AsNoTracking()
                .Where(x =>
                    x.Status == Status.Active &&
                    x.PostStatus == BlogStatus.Publish &&
                    x.BlogPostTags.Any(y =>
                        y.TagId == tagId &&
                        y.Status == Status.Active &&
                        y.Tag.Status == Status.Active))
                .OrderByDescending(x => x.CreatedAt);

            var result = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize + 1)
                .Select(x =>
                    new PublicBlogPostDto
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
                                pt.Status == Status.Active)
                            .Select(pt => pt.Tag)
                            .ToList()
                    })
                .ToListAsync();

            var data = result.Take(pageSize).ToList();
            var hasMore = result.Count > pageSize;

            return (data, hasMore);
        }

        public async Task<PublicBlogPostDto?> GetPublicBlogPostWithTagAsync(Guid blogPostId)
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
                        .ToList(),

                    Comments = x.Comments
                        .Where(c => c.Status == Status.Active)
                        .OrderByDescending(c => c.CreateAt)
                        .ToList()
                })
                .FirstOrDefaultAsync();
                        
        }
    }
}
