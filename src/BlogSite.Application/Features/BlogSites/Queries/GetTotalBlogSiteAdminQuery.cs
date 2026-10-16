using BlogSite.Domain.Enums;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Queries
{
    public class GetTotalBlogSiteAdminQuery : IQuery<int>
    {
        public Status Status { get; set; }
    }
}
