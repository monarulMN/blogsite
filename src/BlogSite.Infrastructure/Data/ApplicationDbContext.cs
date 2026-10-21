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
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<BlogPostCategory> BlogPostCategories { get; set; }
        public DbSet<BlogPostTag> BlogPostTags { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Report> Reports { get; set; }

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


            builder.Entity<Tag>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Status).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
                entity.Property(x => x.UpdatedAt).IsRequired();

                entity.HasIndex(x => x.Name)
                    .IsUnique()
                    .HasFilter("[Status] = 0");
            });


            builder.Entity<BlogPost>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(x => x.Body)
                    .IsRequired();

                entity.Property(x => x.ThumbnailPath)
                    .HasMaxLength(500);

                entity.Property(x => x.PostStatus)
                    .IsRequired();

                entity.Property(x => x.Status)
                    .IsRequired();

                entity.Property(x => x.CreatedAt)
                    .IsRequired();

                entity.Property(x => x.UpdatedAt)
                    .IsRequired();

                entity.HasOne(x => x.Blogsite)
                    .WithMany(x => x.BlogPosts)
                    .HasForeignKey(x => x.BlogSiteId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<BlogPostCategory>(entity =>
            {
                entity.HasKey(x => new { x.BlogPostId, x.CategoryId });

                entity.Property(x => x.CreatedAt).IsRequired();
                entity.Property(x => x.UpdatedAt).IsRequired();

                entity.HasOne(x => x.BlogPost)
                    .WithMany(x => x.BlogPostCategories)
                    .HasForeignKey(x => x.BlogPostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Category)
                    .WithMany(x => x.BlogPostCategories)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            builder.Entity<BlogPostTag>(entity =>
            {
                entity.HasKey(x => new { x.BlogPostId, x.TagId });

                entity.Property(x => x.CreatedAt).IsRequired();
                entity.Property(x => x.UpdatedAt).IsRequired();

                entity.HasOne(x => x.BlogPost)
                    .WithMany(x => x.BlogPostTags)
                    .HasForeignKey(x => x.BlogPostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Tag)
                    .WithMany(x => x.BlogPostTags)
                    .HasForeignKey(x => x.TagId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            builder.Entity<Message>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Text)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(x => x.CreatedAt)
                    .IsRequired();

                entity.HasOne<ApplicationUser>()
                    .WithMany(u => u.SentMessages)
                    .HasForeignKey(m => m.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<ApplicationUser>()
                    .WithMany(u => u.ReceivedMessages)
                    .HasForeignKey(m => m.RecipientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(m => new { m.SenderId, m.RecipientId });
                entity.HasIndex(m => m.CreatedAt);
            });

            builder.Entity<Report>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.ReportText)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(x => x.BlogPostId)
                    .IsRequired();

                entity.HasOne(x => x.BlogPost)
                    .WithMany(x => x.Reports)
                    .HasForeignKey(x => x.BlogPostId)
                    .OnDelete(DeleteBehavior.Cascade);
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
