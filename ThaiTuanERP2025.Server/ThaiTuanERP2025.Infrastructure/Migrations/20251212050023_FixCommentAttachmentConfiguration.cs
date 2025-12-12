using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCommentAttachmentConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentAttachments_FileAttachment_StoredFileId",
                schema: "Core",
                table: "CommentAttachments");

            migrationBuilder.RenameColumn(
                name: "StoredFileId",
                schema: "Core",
                table: "CommentAttachments",
                newName: "FileAttachmentId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentAttachments_StoredFileId",
                schema: "Core",
                table: "CommentAttachments",
                newName: "IX_CommentAttachments_FileAttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentAttachments_FileAttachment_FileAttachmentId",
                schema: "Core",
                table: "CommentAttachments",
                column: "FileAttachmentId",
                principalSchema: "Core",
                principalTable: "FileAttachment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentAttachments_FileAttachment_FileAttachmentId",
                schema: "Core",
                table: "CommentAttachments");

            migrationBuilder.RenameColumn(
                name: "FileAttachmentId",
                schema: "Core",
                table: "CommentAttachments",
                newName: "StoredFileId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentAttachments_FileAttachmentId",
                schema: "Core",
                table: "CommentAttachments",
                newName: "IX_CommentAttachments_StoredFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentAttachments_FileAttachment_StoredFileId",
                schema: "Core",
                table: "CommentAttachments",
                column: "StoredFileId",
                principalSchema: "Core",
                principalTable: "FileAttachment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
