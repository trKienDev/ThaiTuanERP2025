using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFileNameFromFileAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                schema: "Core",
                table: "FileAttachment");

            migrationBuilder.DropColumn(
                name: "FileName",
                schema: "Core",
                table: "FileAttachment");

            migrationBuilder.DropColumn(
                name: "Size",
                schema: "Core",
                table: "FileAttachment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                schema: "Core",
                table: "FileAttachment",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                schema: "Core",
                table: "FileAttachment",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                schema: "Core",
                table: "FileAttachment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
