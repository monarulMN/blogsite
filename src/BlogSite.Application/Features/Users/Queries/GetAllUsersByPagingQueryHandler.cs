using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Users.Queries
{
    public class GetAllUsersByPagingQueryHandler : IQueryHandler<GetAllUsersByPagingQuery, 
        (IList<User> data, int total, int totalDisplay)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public GetAllUsersByPagingQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<(IList<User> data, int total, int totalDisplay)> Handle(GetAllUsersByPagingQuery query,
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.UserRepository.GetPagedUsersAsync(query.PageSize,
                query.PageIndex,
                query.SortOrder,
                query.SearchText,
                cancellationToken);
        }
    }
}
