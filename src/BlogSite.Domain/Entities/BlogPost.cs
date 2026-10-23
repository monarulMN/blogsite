using BlogSite.Domain.Contracts;
using BlogSite.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Domain.Entities
{
    public class BlogPost : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string ThumbnailPath { get; set; } = null!;
        public BlogStatus PostStatus { get; set; } = BlogStatus.Draft;
        public Status Status { get; set; } = Status.Active;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid BlogSiteId { get; set; }
        public Blogsite Blogsite { get; set; }

        public IList<BlogPostCategory> BlogPostCategories { get; set; } 
            = new List<BlogPostCategory>();
        public IList<BlogPostTag> BlogPostTags { get; set; }
            = new List<BlogPostTag>();

        public IList<Report> Reports { get; set; } = new List<Report>();
        public IList<Comment> Comments { get; set; } = new List<Comment>();
    }
}
