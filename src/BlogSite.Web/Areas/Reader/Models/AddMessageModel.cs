using BlogSite.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Areas.Reader.Models
{
    public class AddMessageModel
    {
        [Required]
        public Guid RecipientId { get; set; }

        public UserType UserType { get; set; } = UserType.Reader;

        [Required]
        public string Text { get; set; } = null!;
    }
}
