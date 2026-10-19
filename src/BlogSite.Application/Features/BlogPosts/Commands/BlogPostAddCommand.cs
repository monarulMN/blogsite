using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Commands
{
    public class BlogPostAddCommand : ICommand<BlogPost>
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string ThumbnailPath { get; set; } = null!;
        public Guid BlogSiteId { get; set; }
        public BlogStatus PostStatus { get; set; }
        public IList<Guid> Categories { get; set; } = null!;
        public IList<Guid> Tags { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
