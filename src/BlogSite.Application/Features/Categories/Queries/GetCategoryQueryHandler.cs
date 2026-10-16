using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Queries
{
    public class GetCategoryQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetCategoryQuery, Category?>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<Category?> Handle(GetCategoryQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CategoryRepository.GetByIdAsync(query.Id, 
                cancellationToken);
        }
    }
}
