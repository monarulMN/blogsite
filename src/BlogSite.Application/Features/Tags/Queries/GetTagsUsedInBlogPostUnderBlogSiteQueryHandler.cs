using BlogSite.Application.Contracts;
using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Queries
{
    public class GetTagsUsedInBlogPostUnderBlogSiteQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetTagsUsedInBlogPostUnderBlogSiteQuery, 
            IReadOnlyList<TagDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<IReadOnlyList<TagDto>> Handle(GetTagsUsedInBlogPostUnderBlogSiteQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.TagRepository.GetTagsByBlogSiteAsync(query.BlogSiteId, cancellationToken);
        }
    }
}
