using BlogSite.Domain.Utilities;

namespace BlogSite.Web.Areas.Blogger.Models.Category
{
    public class CategoryListModel : DataTables
    {
        public CategoryAdvanceSearchModel SearchItem { get; set; }
    }
}
