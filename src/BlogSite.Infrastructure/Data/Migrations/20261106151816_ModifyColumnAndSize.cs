using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSite.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyColumnAndSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Blogsites_BlogSiteId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Blogsites_BlogSiteId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "BlogSiteId",
                table: "Comments",
                newName: "BlogsiteId");

            migrationBuilder.RenameColumn(
                name: "BlogSiteId",
                table: "Categories",
                newName: "BlogsiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_Name_BlogSiteId",
                table: "Categories",
                newName: "IX_Categories_Name_BlogsiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_BlogSiteId",
                table: "Categories",
                newName: "IX_Categories_BlogsiteId");

            migrationBuilder.RenameColumn(
                name: "BlogSiteId",
                table: "BlogPosts",
                newName: "BlogsiteId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogPosts_BlogSiteId",
                table: "BlogPosts",
                newName: "IX_BlogPosts_BlogsiteId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Blogsites",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Blogsites",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Blogsites",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_Blogsites_BlogsiteId",
                table: "BlogPosts",
                column: "BlogsiteId",
                principalTable: "Blogsites",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Blogsites_BlogsiteId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Blogsites_BlogsiteId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "BlogsiteId",
                table: "Comments",
                newName: "BlogSiteId");

            migrationBuilder.RenameColumn(
                name: "BlogsiteId",
                table: "Categories",
                newName: "BlogSiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_Name_BlogsiteId",
                table: "Categories",
                newName: "IX_Categories_Name_BlogSiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_BlogsiteId",
                table: "Categories",
                newName: "IX_Categories_BlogSiteId");

            migrationBuilder.RenameColumn(
                name: "BlogsiteId",
                table: "BlogPosts",
                newName: "BlogSiteId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogPosts_BlogsiteId",
                table: "BlogPosts",
                newName: "IX_BlogPosts_BlogSiteId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Blogsites",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Blogsites",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Blogsites",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_Blogsites_BlogSiteId",
                table: "BlogPosts",
                column: "BlogSiteId",
                principalTable: "Blogsites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Blogsites_BlogSiteId",
                table: "Categories",
                column: "BlogSiteId",
                principalTable: "Blogsites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
