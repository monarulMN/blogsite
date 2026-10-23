

namespace BlogSite.Web.Areas.Reader.Models
{
    public class BlogSiteViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public IReadOnlyList<CategoryViewModel> Categories { get; set; }
        public IReadOnlyList<TagViewModel> Tags { get; set; }
    }
}
