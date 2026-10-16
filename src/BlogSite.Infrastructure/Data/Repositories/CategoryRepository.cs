using BlogSite.Application.Contracts.Repositories;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Repositories
{
    public class CategoryRepository : Repository<Category, Guid>, ICategoryRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _applicationDbContext = dbContext;
        }

        public async Task<(IList<Category>, int, int)> GetPagedCategoriesAsync(int pageSize,
            int pageIndex,
            string? sortOrder,
            string? searchText,
            Guid blogSiteId,
            CancellationToken cancellationToken)
        {
            return await GetDynamicAsync(pageIndex: pageIndex, pageSize: pageSize,
                orderBy: sortOrder,
                filter:
                    x => x.Name.Contains(searchText) &&
                    x.Status == Status.Active &&
                    x.BlogSiteId == blogSiteId,
                include: null);
        }

        public async Task<(IList<Category>, int, int)> GetPagedAdminCategoriesAsync(int pageSize,
            int pageIndex,
            string? sortOrder,
            string? searchText,
            CancellationToken cancellationToken)
        {
            return await GetDynamicAsync(pageIndex: pageIndex,
                pageSize: pageSize,
                orderBy: sortOrder,
                filter:
                    x => x.Name.Contains(searchText) &&
                    x.Status == Status.Active,
                include:
                    x => x.Include(b => b.Blogsite));
        }
    }
}
