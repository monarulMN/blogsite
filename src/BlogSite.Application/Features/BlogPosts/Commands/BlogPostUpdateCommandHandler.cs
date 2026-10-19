using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Commands;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Commands
{
    internal class BlogPostUpdateCommandHandler(IMapper mapper, 
        IApplicationUnitOfWork applicationUnitOfWork) 
        : ICommandHandler<BlogPostUpdateCommand, BlogPost>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<BlogPost> Handle(BlogPostUpdateCommand command, 
            CancellationToken cancellationToken)
        {
            var post = await _applicationUnitOfWork.BlogPostRepository.GetByIdAsync(command.Id, cancellationToken);

            _mapper.Map(command, post);
            post.BlogPostCategories.Clear();
            post.BlogPostTags.Clear();

            foreach(var categoryId in command.Categories)
            {
                post.BlogPostCategories.Add(new BlogPostCategory
                {
                    BlogPostId = post.Id,
                    CategoryId = categoryId,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt
                });
            }

            foreach(var tagId in command.Tags)
            {
                post.BlogPostTags.Add(new BlogPostTag
                {
                    BlogPostId = post.Id,
                    TagId = tagId,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt
                });
            }

            await _applicationUnitOfWork.BlogPostRepository.EditAsync(post, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return post;
        }
    }
}
