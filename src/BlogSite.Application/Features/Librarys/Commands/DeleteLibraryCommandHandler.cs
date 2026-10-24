using BlogSite.Application.Contracts;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Librarys.Commands
{
    public class DeleteLibraryCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        ICommandHandler<DeleteLibraryCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<Guid> Handle(DeleteLibraryCommand command, 
            CancellationToken cancellationToken)
        {
            var library = await _applicationUnitOfWork.LibraryRepository.GetLibraryAsync(
                    command.ApplicationUserId, 
                    command.BlogPostId, cancellationToken);

            await _applicationUnitOfWork.LibraryRepository.RemoveAsync(library, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return library.Id;
        }
    }
}
