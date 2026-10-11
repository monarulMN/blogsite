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
    }
}
