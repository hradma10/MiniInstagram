using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniInstagramEF.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentUser_Comments_LikedCommentsId",
                table: "CommentUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentUser_Users_LikedById",
                table: "CommentUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentUser",
                table: "CommentUser");

            migrationBuilder.RenameTable(
                name: "CommentUser",
                newName: "CommentLikes");

            migrationBuilder.RenameIndex(
                name: "IX_CommentUser_LikedCommentsId",
                table: "CommentLikes",
                newName: "IX_CommentLikes_LikedCommentsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentLikes",
                table: "CommentLikes",
                columns: new[] { "LikedById", "LikedCommentsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_Comments_LikedCommentsId",
                table: "CommentLikes",
                column: "LikedCommentsId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_Users_LikedById",
                table: "CommentLikes",
                column: "LikedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_Comments_LikedCommentsId",
                table: "CommentLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_Users_LikedById",
                table: "CommentLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentLikes",
                table: "CommentLikes");

            migrationBuilder.RenameTable(
                name: "CommentLikes",
                newName: "CommentUser");

            migrationBuilder.RenameIndex(
                name: "IX_CommentLikes_LikedCommentsId",
                table: "CommentUser",
                newName: "IX_CommentUser_LikedCommentsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentUser",
                table: "CommentUser",
                columns: new[] { "LikedById", "LikedCommentsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentUser_Comments_LikedCommentsId",
                table: "CommentUser",
                column: "LikedCommentsId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentUser_Users_LikedById",
                table: "CommentUser",
                column: "LikedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
