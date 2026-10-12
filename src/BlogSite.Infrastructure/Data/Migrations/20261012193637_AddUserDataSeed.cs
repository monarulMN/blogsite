using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogSite.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("547a073e-6ee6-4758-a4ac-02c6992250d0"), "547A073E-6EE6-4758-A4AC-02C6992250D0", "Admin", "ADMIN" },
                    { new Guid("5be73deb-2597-4433-ac4b-192b321b06af"), "5BE73DEB-2597-4433-AC4B-192B321B06AF", "Blogger", "BLOGGER" },
                    { new Guid("66507895-f2e5-4fc3-96c8-0f676f2d2280"), "66507895-F2E5-4FC3-96C8-0F676F2D2280", "Reader", "READER" },
                    { new Guid("de73df51-26b6-49f9-9a5f-b9ccb64c1a34"), "DE73DF51-26B6-49F9-9A5F-B9CCB64C1A34", "Support", "SUPPORT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "HasBlog", "IsBlocked", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RegistrationDate", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("23fe4e81-5015-42a0-8d76-d1f08c6b227a"), 0, "23FE4E81-5015-42A0-8D76-D1F08C6B227A", new DateTime(2000, 9, 23, 0, 0, 0, 0, DateTimeKind.Utc), "admin@gmail.com", false, "Monarul", false, false, "Islam", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAELtPfTQCxOzTjT1XtYFxxDNEx8wjHMT7NtWBINbQ5yJ866K7e32K4nbIzIBCvwBB4Q==", null, false, new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Utc), "23FE4E81-5015-42A0-8D76-D1F08C6B227A", false, "admin@gmail.com" },
                    { new Guid("dd19f57f-c1af-480c-b2f3-d39f34e308a2"), 0, "DD19F57F-C1AF-480C-B2F3-D39F34E308A2", new DateTime(1999, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), "support@gmail.com", false, "Support", false, false, "User", false, null, "SUPPORT@GMAIL.COM", "SUPPORT@GMAIL.COM", "AQAAAAIAAYagAAAAEG3/UEMRjIVozINkjgEBKRfOC6kIeNaHw7qqEnMSmHRU+8ZqkFC3117+8cZ5bzo+QQ==", null, false, new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Utc), "DD19F57F-C1AF-480C-B2F3-D39F34E308A2", false, "support@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("547a073e-6ee6-4758-a4ac-02c6992250d0"), new Guid("23fe4e81-5015-42a0-8d76-d1f08c6b227a") },
                    { new Guid("de73df51-26b6-49f9-9a5f-b9ccb64c1a34"), new Guid("dd19f57f-c1af-480c-b2f3-d39f34e308a2") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5be73deb-2597-4433-ac4b-192b321b06af"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("66507895-f2e5-4fc3-96c8-0f676f2d2280"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("547a073e-6ee6-4758-a4ac-02c6992250d0"), new Guid("23fe4e81-5015-42a0-8d76-d1f08c6b227a") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("de73df51-26b6-49f9-9a5f-b9ccb64c1a34"), new Guid("dd19f57f-c1af-480c-b2f3-d39f34e308a2") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("547a073e-6ee6-4758-a4ac-02c6992250d0"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("de73df51-26b6-49f9-9a5f-b9ccb64c1a34"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("23fe4e81-5015-42a0-8d76-d1f08c6b227a"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("dd19f57f-c1af-480c-b2f3-d39f34e308a2"));
        }
    }
}
