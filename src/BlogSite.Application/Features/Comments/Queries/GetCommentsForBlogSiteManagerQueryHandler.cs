using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Comments.Queries
{
    public class GetCommentsForBlogSiteManagerQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetCommentsForBlogSiteManagerQuery, (IList<Comment>, int, int)>
    {

        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<(IList<Comment>, int, int)> Handle(GetCommentsForBlogSiteManagerQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CommentRepository.GetCommentsForBlogSiteManagerAsync(
                query.PageIndex,
                query.PageSize,
                query.SortOrder,
                query.SearchText,
                query.BlogSiteId,
                cancellationToken);
        }
    }
}
