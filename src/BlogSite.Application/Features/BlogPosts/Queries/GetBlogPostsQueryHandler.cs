using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetBlogPostsQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetBlogPostsQuery,
        (IList<BlogPost>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<(IList<BlogPost>, int, int)> Handle(GetBlogPostsQuery query,
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BlogPostRepository.GetPagedBlogPostsAsync(query, 
                cancellationToken);
        }
    }
}
