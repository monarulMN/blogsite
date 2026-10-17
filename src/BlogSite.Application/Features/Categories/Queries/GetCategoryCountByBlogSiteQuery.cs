using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Queries
{
    public class GetCategoryCountByBlogSiteQuery : IQuery<int>
    {
        public Guid BlogSiteId { get; set; }
    }
}
