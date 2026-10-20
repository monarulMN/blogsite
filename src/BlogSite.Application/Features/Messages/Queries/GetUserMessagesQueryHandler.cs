using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Messages.Queries
{
    public class GetUserMessagesQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetUserMessagesQuery, IList<Message>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<IList<Message>> Handle(GetUserMessagesQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.MessageRepository.GetUserMessagesAsync(query.UserId);
        }
    }
}
