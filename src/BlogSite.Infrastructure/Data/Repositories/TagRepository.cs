using BlogSite.Application.Contracts.Repositories;
using BlogSite.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Repositories
{
    public class TagRepository : Repository<Tag, Guid>, ITagRepository
    {
        public TagRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
