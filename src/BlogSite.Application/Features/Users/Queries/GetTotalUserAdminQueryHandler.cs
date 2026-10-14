using BlogSite.Application.Contracts;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Users.Queries
{
    public class GetTotalUserAdminQueryHandler : IQueryHandler<GetTotalUserAdminQuery, int>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public GetTotalUserAdminQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<int> Handle(GetTotalUserAdminQuery query, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.UserRepository.GetCountAsync(query.IsBlocked, cancellationToken);
        }
    }
}
