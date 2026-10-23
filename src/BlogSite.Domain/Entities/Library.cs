using BlogSite.Domain.Contracts;
using BlogSite.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Domain.Entities
{
    public class Library : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public Guid BlogPostId { get; set; }
        public Status Status { get; set; } = Status.Active;
        public DateTime CreatedAt { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
