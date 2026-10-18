using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Queries
{
    public class TagSearchQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<TagSearchQuery, IList<Tag>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<IList<Tag>> Handle(TagSearchQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.TagRepository.GetSearchTagsAsync(query.Name, cancellationToken);
        }
    }
}
