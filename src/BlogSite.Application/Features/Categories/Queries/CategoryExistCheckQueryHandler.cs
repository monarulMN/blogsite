using BlogSite.Application.Contracts;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Queries
{
    public class CategoryExistCheckQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<CategoryExistCheckQuery, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<bool> Handle(CategoryExistCheckQuery query, 
            CancellationToken cancellationToken)
        {
            if (query.Id != Guid.Empty)
                return _applicationUnitOfWork.CategoryRepository.GetCount(x =>
                    x.Name == query.Name &&
                    x.BlogSiteId == query.BlogSiteId &&
                    x.Id != query.Id &&
                    x.Status == Domain.Enums.Status.Active) > 0;

            return _applicationUnitOfWork.CategoryRepository.GetCount(x =>
                    x.Name == query.Name &&
                    x.BlogSiteId == query.BlogSiteId &&
                    x.Status == Domain.Enums.Status.Active) > 0;
        }
    }
}
