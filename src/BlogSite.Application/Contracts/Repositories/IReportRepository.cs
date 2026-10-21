using BlogSite.Domain.Contracts;
using BlogSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Contracts.Repositories
{
    public interface IReportRepository : IRepository<Report, Guid>
    {
        Task<(IList<Report>, int, int)> GetPagedAdminReportsAsync(int pageSize, 
            int pageIndex, 
            string? sortOrder, 
            string? searchText, 
            CancellationToken cancellationToken);
    }
}
