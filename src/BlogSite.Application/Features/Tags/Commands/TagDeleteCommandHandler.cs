using BlogSite.Application.Contracts;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Commands
{
    public class TagDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        ICommandHandler<TagDeleteCommand, Guid>

    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<Guid> Handle(TagDeleteCommand command, 
            CancellationToken cancellationToken)
        {
            var tag = await _applicationUnitOfWork.TagRepository.GetByIdAsync(command.Id, cancellationToken);
            tag.Status = Status.Inactive;

            await _applicationUnitOfWork.TagRepository.RemoveAsync(tag, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return command.Id;
        }
    }
}
