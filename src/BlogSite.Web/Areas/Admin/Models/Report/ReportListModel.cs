using BlogSite.Domain.Utilities;

namespace BlogSite.Web.Areas.Admin.Models.Report
{
    public class ReportListModel : DataTables
    {
        public ReportAdvanceSearchModel SearchItem { get; set; }
    }
}
