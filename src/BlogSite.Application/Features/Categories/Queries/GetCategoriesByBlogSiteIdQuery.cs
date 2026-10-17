using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Queries
{
    public class GetCategoriesByBlogSiteIdQuery : IQuery<IReadOnlyList<CategoryDto>>
    {
        public Guid BlogSiteId { get; set; }
    }
}
