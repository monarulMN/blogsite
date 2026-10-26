using BlogSite.Application.Contracts;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Comments.Queries
{
    public class GetBlogSitesTotalComentsQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetBlogSitesTotalComentsQuery, (int, int)>
    {

        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<(int, int)> Handle(GetBlogSitesTotalComentsQuery query, 
            CancellationToken cancellationToken)
        {
            int approved = await _applicationUnitOfWork.CommentRepository.GetCountAsync(x =>
                x.BlogSiteId == query.BlogSiteId &&
                x.Status == Status.Active);

            int pending = await _applicationUnitOfWork.CommentRepository.GetCountAsync(x =>
                x.BlogSiteId == query.BlogSiteId &&
                x.Status == Status.Inactive);

            return (approved, pending);
        }
    }
}
