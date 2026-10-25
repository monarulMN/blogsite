using BlogSite.Application.Contracts;
using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetPublicBlogPostsByBlogSiteAndTagQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetPublicBlogPostsByBlogSiteAndTagQuery, 
            (IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<(IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)> Handle(
            GetPublicBlogPostsByBlogSiteAndTagQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BlogPostRepository.GetPublicBlogPostsByBlogSiteAndTagAsync(
                query.PageIndex, 
                query.PageSize, 
                query.BlogSiteId, 
                query.TagId, cancellationToken);
        }
    }
}
