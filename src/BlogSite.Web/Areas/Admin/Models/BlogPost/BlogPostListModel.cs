using BlogSite.Domain.Utilities;

namespace BlogSite.Web.Areas.Admin.Models.BlogPost
{
    public class BlogPostListModel : DataTables
    {
        public BlogAdvanceSearchModel SearchItem { get; set; }
    }
}
