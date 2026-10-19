using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Domain.Dtos
{
    public class BlogPostDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
