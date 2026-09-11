using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniInstagramASP.Migrations
{
    /// <inheritdoc />
    public partial class InitAuthPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DomainUserId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DomainUserId",
                table: "AspNetUsers");
        }
    }
}
