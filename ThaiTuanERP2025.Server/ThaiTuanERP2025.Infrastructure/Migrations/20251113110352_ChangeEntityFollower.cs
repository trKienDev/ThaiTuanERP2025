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
                name: "Subject_Type",
                schema: "Core",
                table: "Followers",
                newName: "SubjectType");

            migrationBuilder.RenameColumn(
                name: "Subject_Id",
                schema: "Core",
                table: "Followers",
                newName: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubjectType",
                schema: "Core",
                table: "Followers",
                newName: "Subject_Type");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                schema: "Core",
                table: "Followers",
                newName: "Subject_Id");
        }
    }
}
