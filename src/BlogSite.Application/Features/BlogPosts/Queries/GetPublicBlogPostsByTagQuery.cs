using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetPublicBlogPostsByTagQuery : 
        IQuery<(IReadOnlyList<PublicBlogPostDto> dat, bool HasMore)>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public Guid TagId { get; set; }
    }
}
