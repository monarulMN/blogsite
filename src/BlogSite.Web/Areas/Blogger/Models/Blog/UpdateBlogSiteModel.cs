using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Areas.Blogger.Models.Blog
{
    public class UpdateBlogSiteModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name not should be empty"), 
            MinLength(2), MaxLength(300)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description not should be empty"), 
            MinLength(100), MaxLength(500)]
        public string Description { get; set; } = null!;
    }
}
