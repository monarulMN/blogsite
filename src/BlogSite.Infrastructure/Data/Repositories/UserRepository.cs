using BlogSite.Application.Contracts;
using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Users.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Infrastructure.Identity;
using MapsterMapper;
using Microsoft.Data.SqlClient;
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

        public async Task<(IList<User> data, int total, int totalDisplay)> GetPagedBlockedUsersAsync(int pageIndex,
            int pageSize,
            string? sortOrder,
            string? searchText,
            CancellationToken cancellationToken)
        {
            var result = await GetDynamicAsync(pageIndex: pageIndex, 
                pageSize: pageSize, 
                orderBy: sortOrder, 
                filter: x => 
                    (x.Email.Contains(searchText) || 
                    x.UserName.Contains(searchText)) 
                    && x.IsBlocked != true, 
                include: x => 
                    x.Include(p => p.BlogSite));

            IList<User> users = _mapper.Map<IList<User>>(result.data);

            return (users, result.total, result.totalDisplay);
        }

        public async Task<(IList<User> data, int total, int totalDisplay)> GetPagedUsersAsync(int pageIndex, 
            int pageSize, 
            string? sortOrder,
            string? searchText,
            CancellationToken cancellationToken)
        {
            var result = await GetDynamicAsync(pageIndex: pageIndex,
                pageSize: pageSize,
                orderBy: sortOrder,
                filter: x =>
                    (x.Email.Contains(searchText)) &&
                    x.IsBlocked != true,
                include: x =>
                    x.Include(p => p.BlogSite));

            IList<User> users = _mapper.Map<IList<User>>(result.data);

            return (users, result.total, result.totalDisplay);
        }
    }
}
