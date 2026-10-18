using BlogSite.Application.Contracts.Repositories;
using BlogSite.Application.Features.Tags.Queries;
using BlogSite.Domain.Dtos;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
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

        public async Task<IList<Tag>> GetSearchTagsAsync(string? searchText, 
            CancellationToken cancellationToken)
        {
            return await GetAsync(x => 
                x.Name.Contains(searchText) && 
                x.Status == Status.Active, 
                null, 
                cancellationToken);
        }

        //public async Task<IList<TagDto>> GetTagsByStatusAsync(Status status, 
        //    CancellationToken cancellationToken)
        //{
        //    return await _applicationDbContext.BlogPostTags
        //        .Where(bpt =>
        //            bpt.BlogPost.Status == Status.Active &&
        //            bpt.Status == Status.Active &&
        //            bpt.Tag.Status == Status.Active)
        //        .GroupBy(bpt => new
        //        {
        //            bpt.Tag.Id,
        //            bpt.Tag.Name
        //        })
        //        .Select(g =>
        //            new TagDto
        //            {
        //                Id = g.Key.Id,
        //                Name = g.Key.Name,
        //                PostCount = g.Count()
        //            })
        //        .OrderByDescending(t => t.PostCount)
        //        .ToListAsync();
        //}
    }
}
