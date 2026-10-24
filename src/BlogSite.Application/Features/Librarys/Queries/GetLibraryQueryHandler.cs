using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Librarys.Queries
{
    public class GetLibraryQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetLibraryQuery, IList<Library>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<IList<Library>> Handle(GetLibraryQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.LibraryRepository.GetUserLibraryAsync(
                query.ApplicationUserId, cancellationToken);
        }
    }
}
