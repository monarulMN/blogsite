namespace BlogSite.Web.Areas.Reader.Models
{
    public class BlogSitePostViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public PostViewModel Post { get; set; }
        public IList<CommentViewModel> Comments { get; set; }
        public AddCommentModel Comment { get; set; }
    }
}
