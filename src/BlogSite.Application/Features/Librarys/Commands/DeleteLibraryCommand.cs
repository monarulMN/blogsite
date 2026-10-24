using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Librarys.Commands
{
    public class DeleteLibraryCommand : ICommand<Guid>
    {
        public Guid ApplicationUserId { get; set; }
        public Guid BlogPostId { get; set; }
    }
}
