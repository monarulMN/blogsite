using BlogSite.Application.Contracts;
using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Queries
{
    public class GetTagsByStatusQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetTagsByStatusQuery, IList<TagDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<IList<TagDto>> Handle(GetTagsByStatusQuery query, 
            CancellationToken cancellationToken)
        {
            //return await _applicationUnitOfWork.TagRepository.GetTagsByStatusAsync(query.Status, 
            //    cancellationToken);

            throw new FileNotFoundException();
        }
    }
}
