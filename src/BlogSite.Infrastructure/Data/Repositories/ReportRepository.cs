using BlogSite.Application.Contracts.Repositories;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Repositories
{
    public class ReportRepository : Repository<Report, Guid>, IReportRepository
    {
        public ReportRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<(IList<Report>, int, int)> GetPagedAdminReportsAsync(int pageSize, 
            int pageIndex, 
            string? sortOrder, 
            string? searchText, 
            CancellationToken cancellationToken)
        {
            return await GetDynamicAsync(pageIndex: pageIndex,
                pageSize: pageSize,
                orderBy: sortOrder,
                filter: x =>
                    x.ReportText.Contains(searchText) &&
                    x.Status == Status.Active,
                include: x =>
                    x.Include(b => b.BlogPost)
                        .ThenInclude(s => s.Blogsite));
        }
    }
}
