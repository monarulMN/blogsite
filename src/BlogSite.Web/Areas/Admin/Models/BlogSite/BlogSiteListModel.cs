using BlogSite.Domain.Utilities;

namespace BlogSite.Web.Areas.Admin.Models.BlogSite
{
    public class BlogSiteListModel : DataTables
    {
        public BlogSiteAdvanceSearchModel SearchItem { get; set; }
    }
}
