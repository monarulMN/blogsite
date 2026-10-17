using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Areas.Admin.Models.Tag
{
    public class UpdateTagModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name not should be empty."), Display(Name = "Name:")]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
