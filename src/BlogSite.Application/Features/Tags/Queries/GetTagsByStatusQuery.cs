using BlogSite.Domain.Dtos;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Queries
{
    public class GetTagsByStatusQuery : IQuery<IList<TagDto>>
    {
        public Status Status { get; set; }
    }
}
