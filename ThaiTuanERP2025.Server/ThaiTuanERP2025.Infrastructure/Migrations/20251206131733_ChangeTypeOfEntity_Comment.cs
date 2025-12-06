using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTypeOfEntity_Comment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentCommentId",
                schema: "Core",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_Module_Entity_EntityId",
                schema: "Core",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Entity",
                schema: "Core",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Module",
                schema: "Core",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                schema: "Core",
                table: "Comments",
                newName: "DocumentId");

            migrationBuilder.AddColumn<string>(
                name: "DocumentType",
                schema: "Core",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DocumentType_DocumentId",
                schema: "Core",
                table: "Comments",
                columns: new[] { "DocumentType", "DocumentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentCommentId",
                schema: "Core",
                table: "Comments",
                column: "ParentCommentId",
                principalSchema: "Core",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentCommentId",
                schema: "Core",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_DocumentType_DocumentId",
                schema: "Core",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                schema: "Core",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                schema: "Core",
                table: "Comments",
                newName: "EntityId");

            migrationBuilder.AddColumn<string>(
                name: "Entity",
                schema: "Core",
                table: "Comments",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Module",
                schema: "Core",
                table: "Comments",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Module_Entity_EntityId",
                schema: "Core",
                table: "Comments",
                columns: new[] { "Module", "Entity", "EntityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentCommentId",
                schema: "Core",
                table: "Comments",
                column: "ParentCommentId",
                principalSchema: "Core",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
