using BlogSite.Domain.Utilities;

namespace BlogSite.Web.Areas.Blogger.Models.Post
{
    public class BlogPostListModel : DataTables
    {
        public BlogPostAdvanceSearchModel SearchItem { get; set; }
    }
}
