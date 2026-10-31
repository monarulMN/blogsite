using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Comments.Commands
{
    public class CommentApproveCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        ICommandHandler<CommentApproveCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<Guid> Handle(CommentApproveCommand command, CancellationToken cancellationToken)
        {
            var comment = await _applicationUnitOfWork.CommentRepository.GetByIdAsync(
                command.CommentId, cancellationToken);
            comment.Status = Status.Active;

            await _applicationUnitOfWork.CommentRepository.EditAsync(comment, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return command.CommentId;
        }
                
    }
}
