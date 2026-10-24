using BlogSite.Domain.Contracts;
using BlogSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Contracts.Repositories
{
    public interface ILibraryRepository : IRepository<Library, Guid>
    {
        Task<Library> GetLibraryAsync(Guid applicationUserId, Guid blogPostId, CancellationToken cancellationToken);
        Task<IList<Library>> GetUserLibraryAsync(Guid applicationUserId, 
            CancellationToken cancellationToken);
    }
}
