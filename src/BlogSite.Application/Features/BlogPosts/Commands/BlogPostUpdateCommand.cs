using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Commands
{
    public class BlogPostUpdateCommand : ICommand<BlogPost>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string ThumbnailPath { get; set; } = null!;
        public BlogStatus PostStatus { get; set; }
        public IList<Guid> Categories { get; set; } = null!;
        public IList<Guid> Tags { get; set; } = null!;
        public DateTime UpdatedAt { get; set; }
    }
}
