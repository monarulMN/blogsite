using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetPublicBlogPostsQuery : IQuery<(IReadOnlyList<PublicBlogPostDto> Posts, bool HasMore)>
    {
        public int PageIndex { get; set; } = 1;   // default first load
        public int PageSize { get; set; } = 5;    //default first load
    }
}
