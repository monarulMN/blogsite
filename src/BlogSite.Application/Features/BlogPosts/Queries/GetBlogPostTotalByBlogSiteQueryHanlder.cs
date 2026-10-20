using BlogSite.Application.Contracts;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetBlogPostTotalByBlogSiteQueryHanlder(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetBlogPostTotalByBlogSiteQuery, (int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<(int, int)> Handle(GetBlogPostTotalByBlogSiteQuery query, 
            CancellationToken cancellationToken)
        {
            int published = await _applicationUnitOfWork.BlogPostRepository.GetCountAsync(x =>
                x.BlogSiteId == query.BlogSiteId &&
                x.Status == Status.Active &&
                x.PostStatus == BlogStatus.Publish);

            int draft = await _applicationUnitOfWork.BlogPostRepository.GetCountAsync(x =>
                x.BlogSiteId == query.BlogSiteId &&
                x.Status == Status.Active &&
                x.PostStatus == BlogStatus.Draft);

            return (published, draft);
        }
    }
}
