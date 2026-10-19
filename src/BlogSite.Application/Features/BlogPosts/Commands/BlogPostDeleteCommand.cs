using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogPosts.Commands
{
    public class BlogPostDeleteCommand : ICommand<Guid>
    {
        public Guid Id { get; set; }
    }
}
