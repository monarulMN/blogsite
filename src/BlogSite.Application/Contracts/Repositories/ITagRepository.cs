using BlogSite.Application.Features.Tags.Queries;
using BlogSite.Domain.Contracts;
using BlogSite.Domain.Dtos;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Contracts.Repositories
{
    public interface ITagRepository : IRepository<Tag, Guid>
    {
        Task<(IList<Tag>, int, int)> GetPagedTagsAsync(GetTagsQuery query,
            CancellationToken cancellationToken);

        Task<IList<Tag>> GetSearchTagsAsync(string? searchText, 
            CancellationToken cancellationToken);
        Task<IReadOnlyList<TagDto>> GetTagsByBlogSiteAsync(Guid blogSiteId, 
            CancellationToken cancellationToken);
        Task<IList<TagDto>> GetTagsByStatusAsync(Status status,
            CancellationToken cancellationToken);
    }
}
