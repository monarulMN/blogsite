using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Queries
{
    public class CategoryExistCheckQuery : IQuery<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid BlogSiteId { get; set; }
    }
}
