using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCreatedAtInENTITY_Notification_Reminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreated",
                schema: "Core",
                table: "UserReminders",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                schema: "Core",
                table: "UserNotifications",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "Core",
                table: "UserReminders",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "Core",
                table: "UserNotifications",
                newName: "DateCreated");
        }
    }
}
