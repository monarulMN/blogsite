using BlogSite.Domain.Entities;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Comments.Commands
{
    public class CommentApproveCommand : ICommand<Comment>
    {
    }
}
