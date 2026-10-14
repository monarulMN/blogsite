using BlogSite.Domain.Entities;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Commands
{
    public class BlogSiteActiveCommand : ICommand<Guid>
    {
        public Guid Id { get; set; }
    }
}
