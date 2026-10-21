using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Reports.Commands
{
    public class ReportDeleteCommand : ICommand<Guid>
    {
        public Guid Id { get; set; }
    }
}
