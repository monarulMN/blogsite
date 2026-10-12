using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IUserRepository UserRepository { get; set; }

        public ApplicationUnitOfWork(ApplicationDbContext dbContext,
            IUserRepository userRepository) : base(dbContext)
        {
            UserRepository = userRepository;
        }
    }
}
