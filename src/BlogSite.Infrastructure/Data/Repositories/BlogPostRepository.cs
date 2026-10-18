using BlogSite.Application.Contracts.Repositories;
using BlogSite.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Repositories
{
    public class BlogPostRepository : Repository<BlogPost, Guid>, IBlogPostRepository
    {
        public BlogPostRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
