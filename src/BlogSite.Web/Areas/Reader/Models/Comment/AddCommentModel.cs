using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Areas.Reader.Models.Comment
{
    public class AddCommentModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(150, MinimumLength = 2, 
            ErrorMessage = "Name must be between 2 and 150 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [StringLength(200)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Comment text is required")]
        [StringLength(2000, MinimumLength = 2, 
            ErrorMessage = "Comment must be at least 5 characters")]
        public string Text { get; set; } = null!;

        [Required]
        public Guid BlogPostId { get; set; }

        [Required]
        public Guid BlogSiteId { get; set; }

        public Guid? ParentCommentId { get; set; }
    }
}
