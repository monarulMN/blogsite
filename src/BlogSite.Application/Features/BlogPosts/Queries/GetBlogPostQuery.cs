using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetBlogPostQuery : IQuery<BlogPost>
    {
        public Guid Id { get; set; }
    }
}
