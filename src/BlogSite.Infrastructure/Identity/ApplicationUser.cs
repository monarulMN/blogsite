using BlogSite.Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime RegistrationDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsBlocked { get; set; } = false;
        public bool HasBlog { get; set; } = false;

    }
}
