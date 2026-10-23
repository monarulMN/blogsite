using BlogSite.Domain.Contracts;
using BlogSite.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Domain.Entities
{
    public class Comment : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Text { get; set; } = null!;
        public Guid BlogPostId { get; set; }
        public Guid BlogSiteId { get; set; }
        public BlogPost BlogPost { get; set; } = null!;
        public Status Status { get; set; } = Status.Inactive;
        public DateTime CreateAt { get; set; }

        public Guid? ParentCommentId { get; set; } = null!;
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
