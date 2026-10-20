using BlogSite.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Areas.Admin.Models
{
    public class AddMessageModel
    {
        [Required]
        public Guid RecipientId { get; set; }

        public UserType UserType { get; set; } = UserType.Support;

        [Required]
        public string Text { get; set; } = null!;
    }
}
