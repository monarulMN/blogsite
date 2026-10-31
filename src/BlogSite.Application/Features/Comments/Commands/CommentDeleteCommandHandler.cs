using BlogSite.Application.Contracts;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Comments.Commands
{
    public class CommentDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        ICommandHandler<CommentDeleteCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<Guid> Handle(CommentDeleteCommand command, 
            CancellationToken cancellationToken)
        {
            await _applicationUnitOfWork.CommentRepository.RemoveAsync(x => 
            x.Id == command.CommentId,
            cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return command.CommentId;
        }
    }
}
