using BlogSite.Domain.Entities;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Commands
{
    public class CategoryAddCommand : ICommand<Category>
    {
        public string Name { get; set; } = null!;
        public Guid BlogSiteId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
