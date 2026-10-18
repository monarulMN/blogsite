using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Queries
{
    public class TagSearchQuery : IQuery<IList<Tag>>
    {
        public string? Name { get; set; }
    }
}
