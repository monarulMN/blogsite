using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetPublicBlogPostsByBlogSiteQuery : 
        IQuery<(IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)>
    {
        public Guid BlogSiteId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
