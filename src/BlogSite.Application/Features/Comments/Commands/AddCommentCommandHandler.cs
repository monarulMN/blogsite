using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using Cortex.Mediator.Commands;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Comments.Commands
{
    public class AddCommentCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, 
        IMapper mapper) : ICommandHandler<AddCommentCommand, Comment>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<Comment> Handle(AddCommentCommand command, CancellationToken cancellationToken)
        {
            var comment = _mapper.Map<Comment>(command);
            comment.Id = IdentityGenerator.NewSequentialGuid();

            await _applicationUnitOfWork.CommentRepository.AddAsync(comment, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return comment;
        }
    }
}
