using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Queries
{
    public class GetBlogPostQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        IQueryHandler<GetBlogPostQuery, BlogPost>
    {

        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<BlogPost> Handle(GetBlogPostQuery query, 
            CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BlogPostRepository.GetByIdAsync(query.Id, cancellationToken); 
        }
    }
}
