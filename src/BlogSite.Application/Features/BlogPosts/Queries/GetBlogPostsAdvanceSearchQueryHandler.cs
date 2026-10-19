using BlogSite.Application.Contracts;
using BlogSite.Domain.Dtos;
using Cortex.Mediator.Queries;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetBlogPostsAdvanceSearchQueryHandler(IApplicationUnitOfWork applicationUnitOfWork,
        IMapper mapper) : 
        IQueryHandler<GetBlogPostsAdvanceSearchQuery, 
            (IList<BlogPostDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<(IList<BlogPostDto>, int, int)> Handle(GetBlogPostsAdvanceSearchQuery query, 
            CancellationToken cancellationToken)
        {
            var procedureName = "GetBlogPostSp";
            var output = await _applicationUnitOfWork.SqlUtility.QueryWithStoredProcedureAsync<BlogPostDto>(
                procedureName,
                new Dictionary<string, object>
                {
                    {"PageIndex", query.PageIndex },
                    {"PageSize", query.PageSize },
                    {"SortOrder", query.SortOrder },
                    {"Title", string.IsNullOrWhiteSpace(query.Title) ? null : query.Title},
                    {"DateFrom", query.DateFrom },
                    {"DateTo", query.DateTo }
                },
                new Dictionary<string, Type>
                {
                    {"Total", typeof(int) },
                    {"TotalDisplay", typeof(int) }
                });
            return (output.result, (int)output.outValues["Total"], (int)output.outValues["TotalDisplay"]);
        }
    }
}
