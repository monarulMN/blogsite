using BlogSite.Application.Contracts;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Comments.Queries
{
    public class GetTotalCommentsAdminQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetTotalCommentsAdminQuery, int>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<int> Handle(GetTotalCommentsAdminQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CommentRepository.GetCountAsync(x =>
                x.Status == query.Status, cancellationToken);
        }
    }
}
