using BlogSite.Domain.Utilities;

namespace BlogSite.Web.Areas.Blogger.Models.Comment
{
    public class CommentListModel : DataTables
    {
        public CommentAdvanceSearchModel SearchItem { get; set; }
    }
}
