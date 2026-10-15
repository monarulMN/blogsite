using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Commands;
using MapsterMapper;
using Slugify;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Commands
{
    public class BlogSiteUpdateCommandHandler : ICommandHandler<BlogSiteUpdateCommand, Blogsite>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ISlugHelper _slugHelper;

        public BlogSiteUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,
            IMapper mapper,
            ISlugHelper slugHelper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
            _slugHelper = slugHelper;
        }


        public async Task<Blogsite> Handle(BlogSiteUpdateCommand command, CancellationToken cancellationToken)
        {
            var blogSite = await _applicationUnitOfWork.BlogSiteRepository.GetByIdAsync(command.Id, cancellationToken);
            blogSite = _mapper.Map(command, blogSite);
            blogSite.Slug = _slugHelper.GenerateSlug($"{command.Name}-{blogSite.Id}");

            await _applicationUnitOfWork.BlogSiteRepository.EditAsync(blogSite, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);
            return blogSite;
        }
    }
}
