namespace BlogSite.Web.Areas.Admin.Models
{
    public class DashboardModel
    {
        public int TotalUsers { get; set; }
        public int TotalBlogSites { get; set; }
        public int TotalCategories { get; set; }
        public int TotalTags { get; set; }
        public int TotalPosts { get; set; }
        public int TotalComments { get; set; }
    }
}
