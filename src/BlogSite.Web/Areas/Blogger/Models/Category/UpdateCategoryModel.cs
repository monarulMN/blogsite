using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Areas.Blogger.Models.Category
{
    public class UpdateCategoryModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "category not should be empty!")]
        public string Name { get; set; } = null!;
    }
}
