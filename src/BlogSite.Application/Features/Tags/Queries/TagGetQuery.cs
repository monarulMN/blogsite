using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Queries
{
    public class TagGetQuery : IQuery<Tag>
    {
         public Guid Id { get; set; }
    }
}
