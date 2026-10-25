using BlogSite.Application.Contracts;
using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetPublicBlogPostsQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetPublicBlogPostsQuery, 
        (IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<(IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)> Handle(GetPublicBlogPostsQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BlogPostRepository
                .GetPublicBlogPostAsync(query.PageIndex,
                query.PageSize, cancellationToken);
        }
    }
}
