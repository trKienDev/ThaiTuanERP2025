using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEntityFollower : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubjectType",
                schema: "Core",
                table: "Followers",
                newName: "DocumentType");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                schema: "Core",
                table: "Followers",
                newName: "DocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumentType",
                schema: "Core",
                table: "Followers",
                newName: "SubjectType");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                schema: "Core",
                table: "Followers",
                newName: "SubjectId");
        }
    }
}
