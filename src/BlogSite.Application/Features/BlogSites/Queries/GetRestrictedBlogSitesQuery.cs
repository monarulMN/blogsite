using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Queries
{
    public class GetRestrictedBlogSitesQuery : IQuery<(IList<BlogSiteDto>, int ,int)>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string? SearchText { get; set; }
        public string? SortOrder { get; set; }
    }
}
