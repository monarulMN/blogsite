using BlogSite.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Seeds
{
    public class UserSeed
    {
        public static ApplicationUser[] GetUsers()
        {
            return new ApplicationUser[]
            {
                // 🔹 ADMIN USER
                new ApplicationUser
                {
                    Id = Guid.Parse("23FE4E81-5015-42A0-8D76-D1F08C6B227A"),
                    FirstName = "Monarul",
                    LastName = "Islam",
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    RegistrationDate = new DateTime(2026, 2, 24, 0, 0, 0, DateTimeKind.Utc),
                    DateOfBirth = new DateTime(2000, 09, 23, 0, 0, 0, DateTimeKind.Utc),
                    HasBlog = false,
                    IsBlocked = false,
                    PasswordHash = "AQAAAAIAAYagAAAAELtPfTQCxOzTjT1XtYFxxDNEx8wjHMT7NtWBINbQ5yJ866K7e32K4nbIzIBCvwBB4Q==", //Admin123!!
                    SecurityStamp = "23FE4E81-5015-42A0-8D76-D1F08C6B227A",
                    ConcurrencyStamp = "23FE4E81-5015-42A0-8D76-D1F08C6B227A",
                    LockoutEnabled = false
                },

                new ApplicationUser
                {
                    Id = Guid.Parse("DD19F57F-C1AF-480C-B2F3-D39F34E308A2"),
                    FirstName = "Support",
                    LastName = "User",
                    UserName = "support@gmail.com",
                    NormalizedUserName = "SUPPORT@GMAIL.COM",
                    Email = "support@gmail.com",
                    NormalizedEmail = "SUPPORT@GMAIL.COM",
                    RegistrationDate = new DateTime(2026, 2, 24, 0, 0, 0, DateTimeKind.Utc),
                    DateOfBirth = new DateTime(1999, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                    HasBlog = false,
                    IsBlocked = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEG3/UEMRjIVozINkjgEBKRfOC6kIeNaHw7qqEnMSmHRU+8ZqkFC3117+8cZ5bzo+QQ==", //Support123!!
                    SecurityStamp = "DD19F57F-C1AF-480C-B2F3-D39F34E308A2",
                    ConcurrencyStamp = "DD19F57F-C1AF-480C-B2F3-D39F34E308A2",
                    LockoutEnabled = false
                }
            };
        }
    }
}
