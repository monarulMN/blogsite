using BlogSite.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Seeds
{
    public class RoleSeed
    {
        public static ApplicationRole[] GetRoles()
        {
            return new ApplicationRole[]
            {
                new ApplicationRole
                {
                    Id = Guid.Parse("547A073E-6EE6-4758-A4AC-02C6992250D0"),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "547A073E-6EE6-4758-A4AC-02C6992250D0"
                },
                new ApplicationRole
                {
                    Id = Guid.Parse("DE73DF51-26B6-49F9-9A5F-B9CCB64C1A34"),
                    Name = "Support",
                    NormalizedName = "SUPPORT",
                    ConcurrencyStamp = "DE73DF51-26B6-49F9-9A5F-B9CCB64C1A34"
                },
                new ApplicationRole
                {
                    Id = Guid.Parse("5BE73DEB-2597-4433-AC4B-192B321B06AF"),
                    Name = "Blogger",
                    NormalizedName = "BLOGGER",
                    ConcurrencyStamp = "5BE73DEB-2597-4433-AC4B-192B321B06AF"
                },
                new ApplicationRole
                {
                    Id = Guid.Parse("66507895-F2E5-4FC3-96C8-0F676F2D2280"),
                    Name = "Reader",
                    NormalizedName = "READER",
                    ConcurrencyStamp = "66507895-F2E5-4FC3-96C8-0F676F2D2280"
                }
            };
        }
    }
}
