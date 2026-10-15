using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace BlogSite.Application.Features.BlogSites.Commands
{
    public class BlogSiteDeleteCommand : ICommand<Guid>
    {
        public Guid Id { get; set; }
    }
}
