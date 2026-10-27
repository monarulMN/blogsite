using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Comments.Commands
{
    public class CommentApproveCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        ICommandHandler<CommentApproveCommand, Comment>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public Task<Comment> Handle(CommentApproveCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
