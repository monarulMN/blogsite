using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Areas.Admin.Models.Category
{
    public class UpdateCategoryModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name not should be empty."), Display(Name = "Name:")]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
