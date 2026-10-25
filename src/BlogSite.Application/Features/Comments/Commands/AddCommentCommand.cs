using BlogSite.Domain.Entities;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Comments.Commands
{
    public class AddCommentCommand : ICommand<Comment>
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Text { get; set; } = null!;
        public Guid BlogPostId { get; set; }
        public Guid BlogSiteId { get; set; }
        public Guid? ParentCommentId { get; set; } = null!;
        public DateTime CreateAt { get; set; }
    }
}
