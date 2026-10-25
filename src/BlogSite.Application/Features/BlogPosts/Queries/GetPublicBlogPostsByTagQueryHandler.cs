using BlogSite.Application.Contracts;
using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetPublicBlogPostsByTagQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetPublicBlogPostsByTagQuery, 
            (IReadOnlyList<PublicBlogPostDto> data, bool HasMore)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<(IReadOnlyList<PublicBlogPostDto> data, bool HasMore)> Handle(
            GetPublicBlogPostsByTagQuery query, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BlogPostRepository.GetPublicBlogPostsByTagAsync(
                query.PageIndex, 
                query.PageSize, 
                query.TagId, cancellationToken);
        }
    }
}
