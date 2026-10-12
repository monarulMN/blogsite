using BlogSite.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Infrastructure.Data.Seeds
{
    public class UserRoleSeed
    {
        public static ApplicationUserRole[] GetUserRoles()
        {
            return new ApplicationUserRole[]
            {
                //  Admin user → Admin role
                new ApplicationUserRole
                {
                    UserId = Guid.Parse("23FE4E81-5015-42A0-8D76-D1F08C6B227A"),
                    RoleId = Guid.Parse("547A073E-6EE6-4758-A4AC-02C6992250D0")
                },

                // Support user → Support role
                new ApplicationUserRole
                {
                    UserId = Guid.Parse("DD19F57F-C1AF-480C-B2F3-D39F34E308A2"),
                    RoleId = Guid.Parse("DE73DF51-26B6-49F9-9A5F-B9CCB64C1A34")
                }
            };
        }
    }
}
