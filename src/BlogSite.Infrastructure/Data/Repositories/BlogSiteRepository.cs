using BlogSite.Application.Contracts.Repositories;
using BlogSite.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Repositories
{
    public class BlogSiteRepository : Repository<Blogsite, Guid>, IBlogSiteRepository
    {
        public BlogSiteRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
