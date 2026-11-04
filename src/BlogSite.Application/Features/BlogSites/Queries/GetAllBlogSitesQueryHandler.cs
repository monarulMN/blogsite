using BlogSite.Application.Contracts;
using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Queries
{
    public class GetAllBlogSitesQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetAllBlogSitesQuery, 
            (IList<BlogSiteDto>, int, int)>

    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<(IList<BlogSiteDto>, int, int)> Handle(GetAllBlogSitesQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BlogSiteRepository.GetPagedBlogSiteAsync(query.PageSize, 
                query.PageIndex,  
                query.SortOrder,
                query.SearchText, cancellationToken);
        }
    }
}
