using BlogSite.Application.Contracts.Repositories;
using BlogSite.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Repositories
{
    public class CommentRepository : Repository<Comment, Guid>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<(IList<Comment>, int, int)> GetCommentsForBlogSiteManagerAsync(
            int pageIndex,
            int pageSize,
            string sortOrder,
            string? searchText, 
            Guid blogSiteId, 
            CancellationToken cancellationToken)
        {
            return await GetDynamicAsync(pageIndex: pageIndex, 
                pageSize: pageSize, 
                orderBy: sortOrder, 
                filter: x => 
                    x.Text.Contains(searchText) && 
                    x.BlogSiteId == blogSiteId, 
                include: x => 
                    x.Include(inc => inc.BlogPost));
        }
    }
}
