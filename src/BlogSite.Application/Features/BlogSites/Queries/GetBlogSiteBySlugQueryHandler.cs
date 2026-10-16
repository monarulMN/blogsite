using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Queries
{
    public class GetBlogSiteBySlugQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetBlogSiteBySlugQuery, Blogsite>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<Blogsite> Handle(GetBlogSiteBySlugQuery query, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BlogSiteRepository.GetBlogSiteBySlugAsync(
                query.Slug, cancellationToken);
        }
    }
}
