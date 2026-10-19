using BlogSite.Application.Contracts;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Commands
{
    public class BlogPostDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        ICommandHandler<BlogPostDeleteCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<Guid> Handle(BlogPostDeleteCommand command, 
            CancellationToken cancellationToken)
        {
            var post = await _applicationUnitOfWork.BlogPostRepository
                .GetByIdAsync(command.Id, cancellationToken);

            post.Status = Status.Inactive;

            await _applicationUnitOfWork.BlogPostRepository.RemoveAsync(post, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return command.Id;
        }
    }
}
