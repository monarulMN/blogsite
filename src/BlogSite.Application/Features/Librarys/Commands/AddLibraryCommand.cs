using BlogSite.Domain.Entities;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BlogSite.Application.Features.Librarys.Commands
{
    public class AddLibraryCommand : ICommand<Library>
    {
        public Guid ApplicationUserId { get; set; }
        public Guid BlogPostId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
