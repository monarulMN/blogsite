using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Users.Queries
{
    public class GetAllBlockedUsersQueryHandler : IQueryHandler<GetAllBlockedUsersQuery, 
        (IList<User> data, int total, int totalDisplay)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public GetAllBlockedUsersQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<(IList<User> data, int total, int totalDisplay)> Handle(GetAllBlockedUsersQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.UserRepository.GetPagedBlockedUsersAsync(query, cancellationToken);
        }
    }
}
