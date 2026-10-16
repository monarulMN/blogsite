using BlogSite.Domain.Contracts;
using BlogSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Contracts.Repositories
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {
        Task<(IList<Category>, int, int)> GetPagedCategoriesAsync(int pageSize, 
            int pageIndex, 
            string? sortOrder, 
            string? searchText, 
            Guid blogSiteId, 
            CancellationToken cancellationToken);

        Task<(IList<Category>, int, int)> GetPagedAdminCategoriesAsync(int pageSize, 
            int pageIndex,
            string? sortOrder, 
            string? searchText, 
            CancellationToken cancellationToken);
    }
}
