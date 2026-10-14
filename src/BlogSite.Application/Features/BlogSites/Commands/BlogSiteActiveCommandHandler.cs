using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Commands
{
    public class BlogSiteActiveCommandHandler : ICommandHandler<BlogSiteActiveCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public BlogSiteActiveCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<Guid> Handle(BlogSiteActiveCommand command, CancellationToken cancellationToken)
        {
            await _applicationUnitOfWork.BlogSiteRepository.ActiveAsync(command.Id, cancellationToken);
            return command.Id;
        }
    }
}
