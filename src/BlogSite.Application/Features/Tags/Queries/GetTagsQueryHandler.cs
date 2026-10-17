using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Queries
{
    public class GetTagsQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetTagsQuery, 
            (IList<Tag>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<(IList<Tag>, int, int)> Handle(GetTagsQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.TagRepository.GetPagedTagsAsync(query, cancellationToken);
        }
    }
}
