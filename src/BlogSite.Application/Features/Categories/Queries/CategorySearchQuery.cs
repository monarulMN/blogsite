using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Queries
{
    public class CategorySearchQuery : IQuery<IList<Category>>
    {
        public Guid BlogSiteId { get; set; }
        public string? Name { get; set; }
    }
}
