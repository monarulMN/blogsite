using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Librarys.Queries
{
    public class CheckLibraryQuery : IQuery<bool>
    {
        public Guid ApplicationUserId { get; set; }
        public Guid BlogPostId { get; set; }
    }
}
