using BlogSite.Domain.Contracts;
using BlogSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Contracts.Repositories
{
    public interface ICommentRepository : IRepository<Comment, Guid>
    {
    }
}
