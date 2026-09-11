using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniInstagramEF.Migrations
{
    /// <inheritdoc />
    public partial class PostImageHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgHash",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgHash",
                table: "Posts");
        }
    }
}
