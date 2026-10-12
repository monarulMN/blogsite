using BlogSite.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data
{
    public class ServerTime : IServerTime
    {
        public DateTime DateTime
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}
