using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetPublicBlogPostGetWithTagQuery : IQuery<PublicBlogPostDto>
    {
        public Guid BlogPostId { get; set; }
    }
}
