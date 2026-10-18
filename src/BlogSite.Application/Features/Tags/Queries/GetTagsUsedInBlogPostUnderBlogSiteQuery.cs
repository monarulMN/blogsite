using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Queries
{
    public class GetTagsUsedInBlogPostUnderBlogSiteQuery : IQuery<IReadOnlyList<TagDto>>
    {
        public Guid BlogSiteId { get; set; }
    }
}
