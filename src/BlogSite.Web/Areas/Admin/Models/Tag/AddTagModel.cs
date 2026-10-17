using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Areas.Admin.Models.Tag
{
    public class AddTagModel
    {
        [Required(ErrorMessage = "Name not should be empty."), Display(Name = "Name:")]
        public string Name { get; set; }
    }
}
