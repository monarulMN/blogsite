using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Users.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Infrastructure.Identity;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Repositories
{
    public class UserRepository : Repository<ApplicationUser, Guid>, IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly IServerTime _serverTime;
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext,
            IMapper mapper,
            IServerTime serverTime) 
            : base(dbContext)
        {
            _mapper = mapper;
            _serverTime = serverTime;
            _dbContext = dbContext;
        }

        public async Task<int> GetCountAsync(bool isBlocked, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.Where(x => x.IsBlocked == isBlocked).CountAsync();
        }

        public async Task<(IList<User> data, int total, int totalDisplay)> GetPagedBlockedUsersAsync(GetAllBlockedUsersQuery query, 
            CancellationToken cancellationToken)
        {
            return ((IList<User> data, int total, int totalDisplay))await GetDynamicAsync(
                x => (string.IsNullOrWhiteSpace(query.SearchText) ||
                      x.Email.Contains(query.SearchText) ||
                      (x.UserName != null &&
                       x.UserName.Contains(query.SearchText)))
                     && x.IsBlocked == true,

                query.SortOrder,
                u => u.Include(q => q.BlogSite),
                query.PageIndex,
                query.PageSize,
                true,
                cancellationToken);
        }

        public async Task<(IList<User> data, int total, int totalDisplay)> GetPagedUsersAsync(GetAllUsersByPagingQuery query, 
            CancellationToken cancellationToken)
        {
            return ((IList<User> data, int total, int totalDisplay))await GetDynamicAsync(
                x => (string.IsNullOrWhiteSpace(query.SearchText) ||
                      x.Email.Contains(query.SearchText) ||
                      (x.UserName != null &&
                       x.UserName.Contains(query.SearchText)))
                     && x.IsBlocked == true,

                query.SortOrder,
                u => u.Include(q => q.BlogSite),
                query.PageIndex,
                query.PageSize,
                true,
                cancellationToken);
        }
    }
}
