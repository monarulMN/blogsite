using BlogSite.Application.Contracts.Repositories;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Repositories
{
    public class MessageRepository : Repository<Message, Guid>, IMessageRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public MessageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _applicationDbContext = dbContext;
        }

        public async Task<IList<Message>> GetUserMessagesAsync(Guid id)
        {
            return _applicationDbContext.Messages.Where(m => 
                (m.SenderId == id || 
                m.RecipientId == id) 
                && 
                m.Status == Status.Active).OrderBy(x => x.CreatedAt).ToList();
        }
    }
}
