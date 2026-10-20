using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using Cortex.Mediator.Commands;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Messages.Commands
{
    public class MessageAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,
        IMapper mapper) : 
        ICommandHandler<MessageAddCommand, Message>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Message> Handle(MessageAddCommand command, 
            CancellationToken cancellationToken)
        {
            var message = _mapper.Map<Message>(command);
            message.Id = IdentityGenerator.NewSequentialGuid();

            await _applicationUnitOfWork.MessageRepository.AddAsync(message, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return message;
        }
    }
}
