using BlogSite.Application.Contracts;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Queries
{
    public class GetCategoryCountByBlogSiteQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) :
        IQueryHandler<GetCategoryCountByBlogSiteQuery, int>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<int> Handle(GetCategoryCountByBlogSiteQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CategoryRepository.GetCountAsync(x =>
                x.BlogSiteId == query.BlogSiteId &&
                x.Status == Status.Active);
        }
    }
}
