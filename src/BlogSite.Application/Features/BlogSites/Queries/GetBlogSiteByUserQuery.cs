using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Queries
{
    public class GetBlogSiteByUserQuery : IQuery<Blogsite>
    {
        public Guid AuthorId { get; set; }
    }
}
