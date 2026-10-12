using BlogSite.Application.Contracts.Repositories;
using BlogSite.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Repositories
{
    public class UserRepository : Repository<ApplicationUser, Guid>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
