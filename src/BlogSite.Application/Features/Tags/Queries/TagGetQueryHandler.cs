using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Queries
{
    public class TagGetQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<TagGetQuery, Tag>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<Tag> Handle(TagGetQuery query, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.TagRepository.GetByIdAsync(query.Id, cancellationToken);
        }
    }
}
