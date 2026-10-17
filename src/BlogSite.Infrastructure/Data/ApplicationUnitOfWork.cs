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
        public IBlogSiteRepository BlogSiteRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public ITagRepository TagRepository { get; private set; }

        public ApplicationUnitOfWork(ApplicationDbContext dbContext,
            IUserRepository userRepository,
            IBlogSiteRepository blogSiteRepository,
            ICategoryRepository categoryRepository,
            ITagRepository tagRepository) : base(dbContext)
        {
            UserRepository = userRepository;
            BlogSiteRepository = blogSiteRepository;
            CategoryRepository = categoryRepository;
            TagRepository = tagRepository;
        }
    }
}
