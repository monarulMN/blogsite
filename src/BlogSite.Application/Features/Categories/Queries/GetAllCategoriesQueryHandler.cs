using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Queries
{
    public class GetAllCategoriesQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetAllCategoriesQuery, 
            (IList<Category>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<(IList<Category>, int, int)> Handle(GetAllCategoriesQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CategoryRepository.GetPagedCategoriesAsync(
                query.PageSize,
                query.PageIndex,
                query.SortOrder,
                query.SearchText,
                query.BlogSiteId, cancellationToken);
        }
    }
}
