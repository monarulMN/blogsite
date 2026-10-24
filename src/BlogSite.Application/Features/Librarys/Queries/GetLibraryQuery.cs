using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Librarys.Queries
{
    public class GetLibraryQuery : IQuery<IList<Library>>
    {
        public Guid ApplicationUserId { get; set; }
    }
}
