using BlogSite.Domain.Utilities;
using System.Data;

namespace BlogSite.Web.Areas.Admin.Models.UserManagement
{
    public class UserListModel : DataTables
    {
        public UserAdvancedSearchModel SearchItem { get; set; }
    }
}
