using BlogSite.Application.Features.Users.Queries;
using BlogSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task<(IList<User> data, int total, int totalDisplay)> GetPagedUsersAsync(GetAllUsersByPagingQuery query, 
            CancellationToken cancellationToken);
    }
}
