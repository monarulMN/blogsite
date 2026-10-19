using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetBlogPostsAdvanceSearchQuery : IQuery<(IList<BlogPostDto>, int, int)>
    {
        public string? Title { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public string? SortOrder { get; set; }
    }
}
