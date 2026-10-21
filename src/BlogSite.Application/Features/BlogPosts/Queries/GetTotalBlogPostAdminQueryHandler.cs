using BlogSite.Application.Contracts;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetTotalBlogPostAdminQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetTotalBlogPostAdminQuery, int>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<int> Handle(GetTotalBlogPostAdminQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BlogPostRepository.GetCountAsync(x => x.Status == query.Status);
        }
    }
}
