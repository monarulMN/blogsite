using BlogSite.Domain.Contracts;
using BlogSite.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>, IAggregateRoot<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime RegistrationDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsBlocked { get; set; } = false;
        public bool HasBlog { get; set; } = false;
        public Blogsite? BlogSite { get; set; }

        public ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();

    }
}
