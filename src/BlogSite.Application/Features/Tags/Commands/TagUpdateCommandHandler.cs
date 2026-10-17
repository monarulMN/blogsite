using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Commands;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Commands
{
    public class TagUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, 
        IMapper mapper) 
        : ICommandHandler<TagUpdateCommand, Tag>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Tag> Handle(TagUpdateCommand command, CancellationToken cancellationToken)
        {
            var tag = _mapper.Map<Tag>(command);
            await _applicationUnitOfWork.TagRepository.EditAsync(tag, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return tag;
        }
    }
}
