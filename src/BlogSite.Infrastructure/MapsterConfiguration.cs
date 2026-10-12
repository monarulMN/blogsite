using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure
{
    public class MapsterConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<BlogPostAddCommand, BlogPost>();
            throw new NotImplementedException();
        }
    }
}
