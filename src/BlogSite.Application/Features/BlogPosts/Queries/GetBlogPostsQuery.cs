using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetBlogPostsQuery : IQuery<(IList<BlogPost>, int, int)>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string? SearchText { get; set; }
        public string? SortOrder { get; set; }
        public Guid BlogSiteId { get; set; }
    }
}
