using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Users.Queries
{
    public class GetTotalUserAdminQuery : IQuery<int>
    {
        public bool IsBlocked { get; set; }
    }
}
