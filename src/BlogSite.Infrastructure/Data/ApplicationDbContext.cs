using BlogSite.Infrastructure.Data.Seeds;
using BlogSite.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser,
        ApplicationRole,
        Guid,
        ApplicationUserClaim,
        ApplicationUserRole,
        ApplicationUserLogin,
        ApplicationRoleClaim,
        ApplicationUserToken>(options)
    {

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Seed roles
            builder.Entity<ApplicationRole>().HasData(RoleSeed.GetRoles());
            // Seed users
            builder.Entity<ApplicationUser>().HasData(UserSeed.GetUsers());
            // Seed user roles
            builder.Entity<ApplicationUserRole>().HasData(UserRoleSeed.GetUserRoles());
        }
    }
}
