using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Commands
{
    public class CategoryDeleteCommand : ICommand<Guid>
    {
        public Guid Id { get; set; }
    }
}
