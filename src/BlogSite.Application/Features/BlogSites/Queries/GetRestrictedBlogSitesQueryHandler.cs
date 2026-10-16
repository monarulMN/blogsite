using BlogSite.Application.Contracts;
using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Queries
{
    public class GetRestrictedBlogSitesQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        : IQueryHandler<GetRestrictedBlogSitesQuery, 
            (IList<BlogSiteDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<(IList<BlogSiteDto>, int, int)> Handle(GetRestrictedBlogSitesQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BlogSiteRepository.GetPaagedRestrrictedBlogSiteAsync(query.PageSize,
                query.PageIndex,
                query.SortOrder,
                query.SearchText, cancellationToken);
        }
    }
}
