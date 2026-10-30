using BlogSite.Domain.Utilities;
using BlogSite.Web.Areas.Blogger.Models.Category;

namespace BlogSite.Web.Areas.Blogger.Models
{
    public class CategoryListModel : DataTables
    {
        public CategoryAdvanceSearchModel SearchItem { get; set; }
    }
}
