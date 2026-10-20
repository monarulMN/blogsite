using BlogSite.Domain.Contracts;
using BlogSite.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Domain.Entities
{
    public class Message : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid RecipientId { get; set; }
        public UserType UserType { get; set; }
        public string Text { get; set; } = null!;
        public Status Status { get; set; } = Status.Active;
        public DateTime CreatedAt { get; set; }
    }
}
