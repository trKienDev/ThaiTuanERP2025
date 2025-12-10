using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNavigaitonToCommentMention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentMentions_Users_UserId",
                schema: "Core",
                table: "CommentMentions");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentMentions_Users_UserId",
                schema: "Core",
                table: "CommentMentions",
                column: "UserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentMentions_Users_UserId",
                schema: "Core",
                table: "CommentMentions");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentMentions_Users_UserId",
                schema: "Core",
                table: "CommentMentions",
                column: "UserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
