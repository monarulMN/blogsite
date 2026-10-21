using BlogSite.Domain.Contracts;
using BlogSite.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Domain.Entities
{
    public class Report : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
        public string ReportText { get; set; } = null!;
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public DateTime CreatedAt { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}
