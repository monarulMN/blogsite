using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using Cortex.Mediator.Commands;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Commands
{
    public class TagAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, 
        IMapper mapper) 
        : ICommandHandler<TagAddCommand, Tag>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Tag> Handle(TagAddCommand command, CancellationToken cancellationToken)
        {
            var tag = _mapper.Map<Tag>(command);
            tag.Id = IdentityGenerator.NewSequentialGuid();

            await _applicationUnitOfWork.TagRepository.AddAsync(tag, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return tag;
        }
    }
}
