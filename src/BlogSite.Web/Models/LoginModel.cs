using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.Web.Models
{
    public class LoginModel
    {
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
        public string? ReturnUrl { get; set; }

        
        [TempData]
        public string? ErrorMessage { get; set; }

        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
