using BlogSite.Application.Features.Tags.Queries;
using BlogSite.Domain.Contracts;
using BlogSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Contracts.Repositories
{
    public interface ITagRepository : IRepository<Tag, Guid>
    {
        Task<(IList<Tag>, int, int)> GetPagedTagsAsync(GetTagsQuery query, 
            CancellationToken cancellationToken);
    }
}
