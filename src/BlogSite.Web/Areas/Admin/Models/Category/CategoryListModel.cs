using BlogSite.Domain.Utilities;
using System.Data;

namespace BlogSite.Web.Areas.Admin.Models.Category
{
    public class CategoryListModel : DataTables
    {
        public CategoryAdvanceSearchModel SearchItem { get; set; }
    }
}
