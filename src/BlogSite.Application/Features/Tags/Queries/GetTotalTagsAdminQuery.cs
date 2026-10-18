using BlogSite.Domain.Enums;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Queries
{
    public class GetTotalTagsAdminQuery : IQuery<int>
    {
        public Status Status { get; set; }
    }
}
