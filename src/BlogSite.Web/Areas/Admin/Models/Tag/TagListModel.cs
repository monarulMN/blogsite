using BlogSite.Domain.Utilities;
using System.Data;

namespace BlogSite.Web.Areas.Admin.Models.Tag
{
    public class TagListModel : DataTables
    {
        public TagAdvanceSearchModel SearchItem { get; set; }
    }
}
