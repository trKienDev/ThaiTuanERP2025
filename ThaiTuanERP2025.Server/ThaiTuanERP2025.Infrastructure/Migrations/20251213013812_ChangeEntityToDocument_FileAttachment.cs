using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEntityToDocument_FileAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EntityId",
                schema: "Core",
                table: "FileAttachment",
                newName: "DocumentId");

            migrationBuilder.RenameColumn(
                name: "Entity",
                schema: "Core",
                table: "FileAttachment",
                newName: "Document");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachment_Module_Entity_EntityId",
                schema: "Core",
                table: "FileAttachment",
                newName: "IX_FileAttachment_Module_Document_DocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumentId",
                schema: "Core",
                table: "FileAttachment",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "Document",
                schema: "Core",
                table: "FileAttachment",
                newName: "Entity");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachment_Module_Document_DocumentId",
                schema: "Core",
                table: "FileAttachment",
                newName: "IX_FileAttachment_Module_Entity_EntityId");
        }
    }
}
