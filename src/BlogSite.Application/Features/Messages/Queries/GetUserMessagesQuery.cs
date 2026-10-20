using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Messages.Queries
{
    public class GetUserMessagesQuery : IQuery<IList<Message>>
    {
        public Guid UserId { get; set; }
    }
}
