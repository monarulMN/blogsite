using BlogSite.Application.Contracts;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Queries
{
    public class GetTotalTagsAdminQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetTotalTagsAdminQuery, int>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<int> Handle(GetTotalTagsAdminQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.TagRepository.GetCountAsync(x =>
                x.Status == query.Status, cancellationToken);
        }
    }
}
