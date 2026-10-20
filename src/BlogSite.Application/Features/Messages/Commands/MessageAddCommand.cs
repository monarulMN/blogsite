using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Messages.Commands
{
    public class MessageAddCommand : ICommand<Message>
    {
        public Guid SenderId { get; set; }
        public Guid RecipientId { get; set; }
        public UserType UserType { get; set; }
        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
