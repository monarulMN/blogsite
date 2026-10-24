using BlogSite.Application.Contracts;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Librarys.Queries
{
    public class CheckLibraryQueryHandler(IApplicationUnitOfWork applicationUnitOfWork ) : 
        IQueryHandler<CheckLibraryQuery, bool>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<bool> Handle(CheckLibraryQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.LibraryRepository.GetCountAsync(x =>
                x.ApplicationUserId == query.ApplicationUserId &&
                x.BlogPostId == query.BlogPostId) > 0;
        }
    }
}
