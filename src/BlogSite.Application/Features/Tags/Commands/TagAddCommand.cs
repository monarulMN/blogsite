using BlogSite.Domain.Entities;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace BlogSite.Application.Features.Tags.Commands
{
    public class TagAddCommand : ICommand<Tag>
    {
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
