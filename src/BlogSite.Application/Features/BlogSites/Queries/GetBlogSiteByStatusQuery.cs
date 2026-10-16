using BlogSite.Domain.Entities;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Queries
{
    public class GetBlogSiteByStatusQuery : IQuery<IList<Blogsite>>
    {
        public Status Status { get; set; }
    }
}
