using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Contracts
{
    public interface IServerTime
    {
        DateTime DateTime { get; }
    }
}
