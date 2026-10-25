using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetPublicBlogPostsByBlogSiteAndCategoryQuery : 
        IQuery<(IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)>
    {
        public Guid CategoryId { get; set; }
        public Guid BlogSiteId { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
