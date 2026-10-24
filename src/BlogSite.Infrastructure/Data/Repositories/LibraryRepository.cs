using BlogSite.Application.Contracts.Repositories;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Repositories
{
    public class LibraryRepository : Repository<Library, Guid>, ILibraryRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public LibraryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _applicationDbContext = dbContext;
        }

        public async Task<Library> GetLibraryAsync(Guid applicationUserId, Guid blogPostId, 
            CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Libraries.FirstOrDefaultAsync(x =>
                x.ApplicationUserId == applicationUserId &&
                x.BlogPostId == blogPostId,
                cancellationToken);
        }

        public async Task<IList<Library>> GetUserLibraryAsync(Guid applicationUserId, 
            CancellationToken cancellationToken)
        {
            return await GetAsync(x => 
                x.ApplicationUserId == applicationUserId && 
                x.Status == Status.Active, 
                include: x => 
                    x.Include(x => x.BlogPost), cancellationToken);
        }
    }
}
