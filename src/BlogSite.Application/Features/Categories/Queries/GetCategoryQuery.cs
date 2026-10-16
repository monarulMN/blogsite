using BlogSite.Domain.Entities;
using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Queries
{
    public class GetCategoryQuery : IQuery<Category?>
    {
        public Guid Id { get; set; }
    }
}
