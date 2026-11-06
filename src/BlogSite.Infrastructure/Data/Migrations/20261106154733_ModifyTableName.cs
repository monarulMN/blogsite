using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSite.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Blogsites_BlogsiteId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogsites_AspNetUsers_AuthorId",
                table: "Blogsites");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Blogsites_BlogsiteId",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Blogsites",
                table: "Blogsites");

            migrationBuilder.RenameTable(
                name: "Blogsites",
                newName: "BlogShareSites");

            migrationBuilder.RenameIndex(
                name: "IX_Blogsites_Slug",
                table: "BlogShareSites",
                newName: "IX_BlogShareSites_Slug");

            migrationBuilder.RenameIndex(
                name: "IX_Blogsites_AuthorId",
                table: "BlogShareSites",
                newName: "IX_BlogShareSites_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogShareSites",
                table: "BlogShareSites",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_BlogShareSites_BlogsiteId",
                table: "BlogPosts",
                column: "BlogsiteId",
                principalTable: "BlogShareSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogShareSites_AspNetUsers_AuthorId",
                table: "BlogShareSites",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_BlogShareSites_BlogsiteId",
                table: "Categories",
                column: "BlogsiteId",
                principalTable: "BlogShareSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_BlogShareSites_BlogsiteId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogShareSites_AspNetUsers_AuthorId",
                table: "BlogShareSites");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_BlogShareSites_BlogsiteId",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogShareSites",
                table: "BlogShareSites");

            migrationBuilder.RenameTable(
                name: "BlogShareSites",
                newName: "Blogsites");

            migrationBuilder.RenameIndex(
                name: "IX_BlogShareSites_Slug",
                table: "Blogsites",
                newName: "IX_Blogsites_Slug");

            migrationBuilder.RenameIndex(
                name: "IX_BlogShareSites_AuthorId",
                table: "Blogsites",
                newName: "IX_Blogsites_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Blogsites",
                table: "Blogsites",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_Blogsites_BlogsiteId",
                table: "BlogPosts",
                column: "BlogsiteId",
                principalTable: "Blogsites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogsites_AspNetUsers_AuthorId",
                table: "Blogsites",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Blogsites_BlogsiteId",
                table: "Categories",
                column: "BlogsiteId",
                principalTable: "Blogsites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
