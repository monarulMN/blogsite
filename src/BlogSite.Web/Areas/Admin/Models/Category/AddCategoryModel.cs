using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Areas.Admin.Models.Category
{
    public class AddCategoryModel
    {
        [Required(ErrorMessage = "Name not should be empty."), Display(Name = "Name:")]
        public string Name { get; set; }
        public Guid BlogSiteId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
