namespace BlogSite.Web.Areas.Blogger.Models
{
    public class BlogManagementModel
    {
        public int TotalCategories { get; set; }
        public int PublishedPosts { get; set; }
        public int DraftPosts { get; set; }
        public int ApprovedComments { get; set; }
        public int PendingComments { get; set; }
        public string BlogSiteSlug { get; set; }
    }
}
