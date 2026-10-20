using BlogSite.Application.Features.Users.Queries;
using BlogSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task<int> GetCountAsync(bool isBlocked, CancellationToken cancellationToken);
        Task<(IList<User> data, int total, int totalDisplay)> GetPagedBlockedUsersAsync(GetAllBlockedUsersQuery query, 
            CancellationToken cancellationToken);
        Task<(IList<User> data, int total, int totalDisplay)> GetPagedUsersAsync(int pageIndex, 
            int pageSize, 
            string? searchText, 
            string? sortOrder, 
            CancellationToken cancellationToken);
    }
}
