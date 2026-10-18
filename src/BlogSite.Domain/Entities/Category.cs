using BlogSite.Domain.Contracts;
using BlogSite.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Domain.Entities
{
    public class Category : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Status Status { get; set; } = Status.Active;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid BlogSiteId { get; set; }
        public Blogsite Blogsite { get; set; } = null!;

        public IList<BlogPostCategory> BlogPostCategories { get; set; }
            = new List<BlogPostCategory>();
    }
}
