using BlogSite.Application.Contracts;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Queries
{
    public class GetTotalBlogSiteAdminQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetTotalBlogSiteAdminQuery, int>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<int> Handle(GetTotalBlogSiteAdminQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BlogSiteRepository.GetCountAsync(
                x => x.Status == query.Status, cancellationToken);
        }
    }
}
