namespace BlogSite.Web.Areas.Reader.Models.Library
{
    public class AddLibraryModel
    {
        public Guid ApplicationUserId { get; set; }
        public Guid BlogPostId { get; set; }
    }
}
