using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Queries
{
    public class GetBlogSiteByStatusQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) :
        IQueryHandler<GetBlogSiteByStatusQuery, IList<Blogsite>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<IList<Blogsite>> Handle(GetBlogSiteByStatusQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BlogSiteRepository.GetBlogSiteByStatusAsync(
                query.Status, cancellationToken);
        }
    }
}
