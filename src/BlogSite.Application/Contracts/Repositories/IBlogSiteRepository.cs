using BlogSite.Domain.Contracts;
using BlogSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Contracts.Repositories
{
    public interface IBlogSiteRepository : IRepository<Blogsite, Guid>
    {
        Task<bool> ActiveAsync(Guid id, CancellationToken cancellationToken);
        
    }
}
