using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Queries
{
    public class GetBlogSiteByUserQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetBlogSiteByUserQuery, Blogsite>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<Blogsite> Handle(GetBlogSiteByUserQuery query,
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BlogSiteRepository.GetByAuthorIdAsync(
                query.AuthorId, cancellationToken);
        }
    }
}
