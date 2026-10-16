using BlogSite.Infrastructure.Data.Seeds;
using BlogSite.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlogSite.Domain.Entities;

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
        public DbSet<Blogsite> Blogsites { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.Status).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
                entity.Property(x => x.UpdatedAt).IsRequired();

                entity.HasOne(x => x.Blogsite)
                    .WithMany(x => x.Categories)
                    .HasForeignKey(x => x.BlogSiteId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(x => new { x.Name, x.BlogSiteId })
                    .IsUnique()
                    .HasFilter("[Status] = 0");
                    
            });


            builder.Entity<Blogsite>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();

                entity.Property(e => e.Status).IsRequired();

                entity.HasIndex(e => e.Slug).IsUnique();

                entity.HasOne<ApplicationUser>()
                    .WithOne(e => e.BlogSite)
                    .HasForeignKey<Blogsite>(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

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
