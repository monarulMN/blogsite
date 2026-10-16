using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.BlogSites.Queries;
using BlogSite.Domain.Dtos;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;

namespace BlogSite.Infrastructure.Data.Repositories
{
    public class BlogSiteRepository : Repository<Blogsite, Guid>, IBlogSiteRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public BlogSiteRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _applicationDbContext = dbContext;
        }

        public async Task<bool> ActiveAsync(Guid id, CancellationToken cancellationToken)
        {
            var blogSite = await _applicationDbContext.Blogsites
                .FirstOrDefaultAsync(x => x.Id == id);

            if (blogSite is null)
                return false;
            var now = DateTime.UtcNow;

            blogSite.Status = Status.Active;
            blogSite.UpdatedAt = now;

            //var posts = await _applicationDbContext.BlogPosts
            //    .Where(x => x.BlogSiteId == id)
            //    .ToListAsync();

            //foreach(var post in posts)
            //{
            //    post.Status = Status.Active;
            //    post.UpdatedAt = now;
            //}

            //var categories = await _applicationDbContext.Categories
            //    .Where(x => x.BlogSiteId == id)
            //    .ToListAsync();

            //foreach(var category in categories)
            //{
            //    category.Status = Status.Active;
            //    category.UpdatedAt = now;
            //}

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var blogSite = await _applicationDbContext.Blogsites
                .FirstOrDefaultAsync(x => x.Id == id);

            if (blogSite is null)
                return false;

            var now = DateTime.UtcNow;

            blogSite.Status = Status.Inactive;
            blogSite.UpdatedAt = now;

            //var posts = await _applicationDbContext.Blogsites
            //    .Where(x => x.BlogSiteId == id)
            //    .ToListAsync();

            //foreach(var post in posts)
            //{
            //    post.Status = Status.Inactive;
            //    post.UpdatedAt = now;
            //}

            //var categories = await _applicationDbContext.Blogsites
            //    .Where(x => x.BlogSiteId == id)
            //    .ToListAsync();

            //foreach(var category in categories)
            //{
            //    category.Status = Status.Inactive;
            //    category.UpdatedAt = now;
            //}

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<Blogsite> GetBlogSiteBySlugAsync(string slug, CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Blogsites.FirstAsync(s => s.Slug == slug, cancellationToken);
        }

        public async Task<IList<Blogsite>> GetBlogSiteByStatusAsync(Status status, CancellationToken cancellationToken)
        {
            int count = 20;
            var query = _applicationDbContext.Blogsites.Where(x => x.Status == status);

            var total = await query.CountAsync();

            if (total <= count)
                return await query.ToListAsync();

            var random = new Random();
            var skip = random.Next(0, total - count);

            var randomSites = await query
                .Skip(skip)
                .Take(count)
                .ToListAsync();

            return randomSites;
        }

        public async Task<Blogsite> GetByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Blogsites.FirstOrDefaultAsync(b => b.AuthorId == authorId,
                cancellationToken);
        }

        public async Task<(IList<BlogSiteDto>, int, int)> GetPaagedRestrrictedBlogSiteAsync(int pageSize, 
            int pageIndex, 
            string? sortOrder, 
            string? searchText, 
            CancellationToken cancellationToken)
        {
            var result = await (
                from blog in _applicationDbContext.Blogsites
                join user in _applicationDbContext.Users
                    on blog.AuthorId equals user.Id
                where blog.Name.Contains(searchText)
                      && blog.Status == Status.Inactive
                select new
                {
                    BlogSiteId = blog.Id,
                    BlogSiteName = blog.Name,

                    AuthorName = $"{user.FirstName} {user.LastName}",
                    AuthorEmail = user.Email,

                    //TotalPosts = _applicationDbContext.BlogPosts
                    //    .Count(p => p.BlogSiteId == blog.Id && p.Status == Status.Inactive),

                    //TotalCategories = _applicationDbContext.Categories
                    //    .Count(c => c.BlogSiteId == blog.Id && c.Status == Status.Inactive),
                    CreatedAt = blog.CreatedAt
                }
            )
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            IList<BlogSiteDto> data = result.Select(r => new BlogSiteDto
            {
                Id = r.BlogSiteId,
                Name = r.BlogSiteName,
                UserName = r.AuthorName,
                Email = r.AuthorEmail,
                //TotalPost = r.TotalPosts,
                //TotalCategory = r.TotalCategories,
                CreatedAt = r.CreatedAt
            }).ToList();

            var toatlSites = await GetDynamicAsync(pageIndex: pageIndex, 
                pageSize: pageSize, 
                orderBy: sortOrder, 
                filter: 
                    x => x.Name.Contains(searchText) && 
                    x.Status == Status.Active);

            return (data, toatlSites.total, toatlSites.totalDisplay);
        }

        public async Task<(IList<BlogSiteDto>, int, int)> GetPagedBlogSiteAsync(int pageIndex, 
            int pageSize, 
            string? searchText, 
            string? sortOrder, 
            CancellationToken cancellationToken)
        {
            var result = await (
                from blog in _applicationDbContext.Blogsites
                join user in _applicationDbContext.Users
                    on blog.AuthorId equals user.Id
                where blog.Name.Contains(searchText)
                    && blog.Status == Status.Inactive
                select new
                {
                    BlogSiteId = blog.Id,
                    BlogSiteName = blog.Name,

                    AuthorName = $"{user.FirstName} {user.LastName}",
                    AuthorEmail = user.Email,

                    //TotalPosts = _applicationDbContext.BlogPosts
                    //    .Count(c => c.BlogSiteId == blog.Id && c.Status == Status.Inactive),

                    //TotalCategories = _applicationDbContext.Categories
                    //    .Count(c => c.BlogSiteId == blog.Id && c.Status == Status.Inactive),
                    CreatedAt = blog.CreatedAt

                })
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            IList<BlogSiteDto> data = result.Select(r => new BlogSiteDto
            {
                Id = r.BlogSiteId,
                Name = r.BlogSiteName,
                UserName = r.AuthorName,
                Email = r.AuthorEmail,
                //TotalPost = r.TotalPosts,
                //TotalCategory = r.TotalCategories,
                CreatedAt = r.CreatedAt
            }).ToList();

            var totalSites = await GetDynamicAsync(pageIndex: pageIndex,
                pageSize: pageSize,
                orderBy: sortOrder,
                filter:
                    x => x.Name.Contains(searchText) &&
                    x.Status == Status.Active);

            return (data, totalSites.total, totalSites.totalDisplay);
        }
    }
}
