using BlogSite.Domain.Entities;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Reports.Commands
{
    public class AddReportCommand : ICommand<Report>
    {
        public Guid BlogPostId { get; set; }
        public string ReportText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
