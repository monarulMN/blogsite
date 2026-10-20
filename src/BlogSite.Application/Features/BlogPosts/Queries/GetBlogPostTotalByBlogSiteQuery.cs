using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetBlogPostTotalByBlogSiteQuery : IQuery<(int, int)>
    {
        public Guid BlogSiteId { get; set; }
    }
}
