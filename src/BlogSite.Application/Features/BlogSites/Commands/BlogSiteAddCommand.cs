using BlogSite.Domain.Entities;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.BlogSites.Commands
{
    public class BlogSiteAddCommand : ICommand<Blogsite>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
