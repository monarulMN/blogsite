using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Comments.Queries
{
    public class GetBlogSitesTotalComentsQuery : IQuery<(int, int)>
    {
        public Guid BlogSiteId { get; set; }
    }
}
