using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Tags.Commands
{
    public class TagDeleteCommand : ICommand<Guid>
    {
        public Guid Id { get; set; }
    }
}
