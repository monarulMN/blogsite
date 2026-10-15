using BlogSite.Application.Features.BlogSites.Queries;
using BlogSite.Domain.Contracts;
using BlogSite.Domain.Dtos;
using BlogSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Contracts.Repositories
{
    public interface IBlogSiteRepository : IRepository<Blogsite, Guid>
    {
        Task<bool> ActiveAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<(IList<BlogSiteDto>, int, int)> GetPagedBlogSiteAsync(int pageIndex, 
            int pageSize, 
            string? searchText, 
            string? sortOrder, CancellationToken cancellationToken);
    }
}
