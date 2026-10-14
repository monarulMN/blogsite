using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Areas.Admin.Models.UserManagement
{
    public class AddUserRoleModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
