using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Tags.Queries;
using BlogSite.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Repositories
{
    public class TagRepository : Repository<Tag, Guid>, ITagRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TagRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _applicationDbContext = dbContext;
        }

        public async Task<(IList<Tag>, int, int)> GetPagedTagsAsync(GetTagsQuery query, 
            CancellationToken cancellationToken)
        {
            return await GetDynamicAsync(x => 
                (
                    query.SearchText == null || 
                    x.Name.Contains(query.SearchText)
                ) && x.Status == Domain.Enums.Status.Active,
                query.SortOrder,
                null,
                query.PageIndex,
                query.PageSize,
                true,
                cancellationToken);
        }
    }
}
