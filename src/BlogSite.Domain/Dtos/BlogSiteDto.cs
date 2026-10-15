using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Domain.Dtos
{
    public class BlogSiteDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int TotalCategory { get; set; } 
        public int TotalPost { get; set; } 
        public DateTime CreatedAt { get; set; } 
    }
}
