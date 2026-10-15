using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using Cortex.Mediator.Commands;
using MapsterMapper;
using Slugify;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Commands
{
    public class BlogSiteAddCommandHandler : ICommandHandler<BlogSiteAddCommand, Blogsite>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ISlugHelper _slugHelper;

        public BlogSiteAddCommandHandler( IApplicationUnitOfWork applicationUnitOfWork,
            IMapper mapper,
            ISlugHelper slugHelper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
            _slugHelper = slugHelper;
        }


        public async Task<Blogsite> Handle(BlogSiteAddCommand command, CancellationToken cancellationToken)
        {
            var blogSite = _mapper.Map<Blogsite>(command);
            blogSite.Id = IdentityGenerator.NewSequentialGuid();
            blogSite.Slug = _slugHelper.GenerateSlug($"{command.Name}-{blogSite.Id}");

            await _applicationUnitOfWork.BlogSiteRepository.AddAsync(blogSite, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);
            return blogSite;
        }
    }
}
