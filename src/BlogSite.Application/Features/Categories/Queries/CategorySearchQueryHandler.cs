using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Queries
{
    public class CategorySearchQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<CategorySearchQuery, IList<Category>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<IList<Category>> Handle(CategorySearchQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CategoryRepository.GetSearchCategoriesAsync(
                query.Name, query.BlogSiteId, cancellationToken);
        }
    }
}
