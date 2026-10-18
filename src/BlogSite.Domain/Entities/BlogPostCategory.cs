using BlogSite.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Domain.Entities
{
    public class BlogPostCategory
    {
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; } = null!;

        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Status Status { get; set; } = Status.Active;
    }
}
