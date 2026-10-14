using BlogSite.Application.Contracts.Repositories;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
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
    }
}
