using BlogSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace BlogSite.Domain.Dtos
{
    public class PublicBlogPostDto
    {
        public Guid Id { get; set; }
        public string Author { get; set; } = null!;
        public string AuthorSlag { get; set; } = null!;
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string ThumbnailPath { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public List<Tag> Tags { get; set; } = new();
        //public List<Comment> Comments { get; set; } = new();
    }
}
