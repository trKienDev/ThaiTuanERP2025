using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsPublic_StoredFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StoredFiles_IsPublic",
                schema: "Files",
                table: "StoredFiles");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                schema: "Files",
                table: "StoredFiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                schema: "Files",
                table: "StoredFiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_StoredFiles_IsPublic",
                schema: "Files",
                table: "StoredFiles",
                column: "IsPublic");
        }
    }
}
