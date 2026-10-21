using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Reports.Queries
{
    public class GetReportsAdminQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetReportsAdminQuery, 
            (IList<Report>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public Task<(IList<Report>, int, int)> Handle(GetReportsAdminQuery query, 
            CancellationToken cancellationToken)
        {
            return _applicationUnitOfWork.ReportRepository.GetPagedAdminReportsAsync(query.PageSize, 
                query.PageIndex, 
                query.SortOrder, 
                query.SearchText, cancellationToken);
        }
    }
}
