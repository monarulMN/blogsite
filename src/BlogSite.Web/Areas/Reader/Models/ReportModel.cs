using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Areas.Reader.Models
{
    public class ReportModel
    {
        [Required]
        public Guid BlogPostId { get; set; }

        [Required]
        public string ReportText { get; set; } = null!;
    }
}
