using BlogSite.Application.Contracts;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Commands
{
    public class BlogSiteDeleteCommandHanlder : ICommandHandler<BlogSiteDeleteCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public BlogSiteDeleteCommandHanlder(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<Guid> Handle(BlogSiteDeleteCommand command, CancellationToken cancellationToken)
        {
            await _applicationUnitOfWork.BlogSiteRepository.DeleteAsync(command.Id, cancellationToken);
            return command.Id;
        }
    }
}
