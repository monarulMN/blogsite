using BlogSite.Application.Contracts;
using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Queries
{
    public class GetAllBlogSitesQueriesHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetAllBlogSitesQueries, 
            (IList<BlogSiteDto>, int, int)>

    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<(IList<BlogSiteDto>, int, int)> Handle(GetAllBlogSitesQueries query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BlogSiteRepository.GetPagedBlogSiteAsync(query.PageIndex, 
                query.PageSize, 
                query.SearchText, 
                query.SortOrder, cancellationToken);
        }
    }
}
