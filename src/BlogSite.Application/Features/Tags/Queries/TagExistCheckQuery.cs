using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Queries
{
    public class TagExistCheckQuery : IQuery<bool>
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
