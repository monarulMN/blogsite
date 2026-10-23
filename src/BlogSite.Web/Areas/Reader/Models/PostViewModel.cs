namespace BlogSite.Web.Areas.Reader.Models
{
    public class PostViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string ThumbnailPath { get; set; } = null!;
        public IReadOnlyList<TagViewModel> Tags { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
