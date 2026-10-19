using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using Cortex.Mediator.Commands;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Commands
{
    public class BlogPostAddCommandHandler(IMapper mapper,
        IApplicationUnitOfWork applicationUnitOfWork) 
        : ICommandHandler<BlogPostAddCommand, BlogPost>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<BlogPost> Handle(BlogPostAddCommand command, 
            CancellationToken cancellationToken)
        {
            var blogPost = _mapper.Map<BlogPost>(command);
            blogPost.Id = IdentityGenerator.NewSequentialGuid();
            
            foreach(var categoryId in command.Categories)
            {
                blogPost.BlogPostCategories.Add(new BlogPostCategory
                {
                    BlogPostId = blogPost.Id,
                    CategoryId = categoryId,
                    CreatedAt = command.CreatedAt,
                    UpdatedAt = command.UpdatedAt
                });
            }

            foreach(var tagId in command.Tags)
            {
                blogPost.BlogPostTags.Add(new BlogPostTag
                {
                    BlogPostId = blogPost.Id,
                    TagId = tagId,
                    CreatedAt = command.CreatedAt,
                    UpdatedAt = command.UpdatedAt
                });
            }

            await _applicationUnitOfWork.BlogPostRepository.AddAsync(blogPost, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);
            return blogPost;
        }
    }
}
