using BlogSite.Application.Contracts;
using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetPublicBlogPostGetWithTagQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) :
        IQueryHandler<GetPublicBlogPostGetWithTagQuery, PublicBlogPostDto>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<PublicBlogPostDto> Handle(GetPublicBlogPostGetWithTagQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BlogPostRepository
                .GetPublicBlogPostWithTagAsync(query.BlogPostId);
        }
    }
}
