using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Areas.Blogger.Models.Category
{
    public class AddCategoryModel
    {
        [Required(ErrorMessage = "category not should be empty!")]
        public string Name { get; set; } = null!;
    }
}
