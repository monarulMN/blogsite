using BlogSite.Domain.Contracts;
using BlogSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Contracts.Repositories
{
    public interface IMessageRepository : IRepository<Message, Guid>
    {
        Task<IList<Message>> GetUserMessagesAsync(Guid id);
    }
}
