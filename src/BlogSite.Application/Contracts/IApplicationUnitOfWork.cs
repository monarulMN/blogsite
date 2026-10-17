using BlogSite.Application.Contracts.Repositories;
using BlogSite.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Contracts
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepository { get; set; }
        public IBlogSiteRepository BlogSiteRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; }
        public ITagRepository TagRepository { get; }
    }
}
