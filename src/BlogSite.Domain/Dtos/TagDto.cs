using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Domain.Dtos
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int PostCount { get; set; }
    }
}
