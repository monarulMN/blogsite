using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BlogSite.Application.Features.Comments.Commands
{
    public class CommentDeleteCommand : ICommand<Guid>
    {
        public Guid CommentId { get; set; }
    }
}
