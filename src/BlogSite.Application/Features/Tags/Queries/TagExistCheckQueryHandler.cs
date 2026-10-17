using BlogSite.Application.Contracts;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Queries
{
    public class TagExistCheckQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<TagExistCheckQuery, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<bool> Handle(TagExistCheckQuery query, 
            CancellationToken cancellationToken)
        {
            if (query.Id == Guid.Empty)
                return _applicationUnitOfWork.TagRepository.GetCount(x => 
                    x.Name == query.Name) > 0;

            return _applicationUnitOfWork.TagRepository.GetCount(x => 
                x.Name == query.Name && 
                x.Id != query.Id && 
                x.Status == Status.Active) > 0;
        }
    }
}
