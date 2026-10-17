using BlogSite.Application.Contracts;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Queries
{
    public class GetTotalCategoriesAdminQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) :
        IQueryHandler<GetTotalCategoriesAdminQuery, int>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<int> Handle(GetTotalCategoriesAdminQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CategoryRepository.GetCountAsync(x =>
                x.Status == query.Status);
        }
    }
}
