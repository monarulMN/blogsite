using BlogSite.Application.Contracts;
using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Queries
{
    public class GetCategoriesByBlogSiteIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) :
        IQueryHandler<GetCategoriesByBlogSiteIdQuery,
            IReadOnlyList<CategoryDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<IReadOnlyList<CategoryDto>> Handle(GetCategoriesByBlogSiteIdQuery query, 
            CancellationToken cancellationToken)
        {
            return (IReadOnlyList<CategoryDto>)await _applicationUnitOfWork.CategoryRepository
                .GetCategoryByBlogSiteId(query.BlogSiteId);
        }
    }
}
