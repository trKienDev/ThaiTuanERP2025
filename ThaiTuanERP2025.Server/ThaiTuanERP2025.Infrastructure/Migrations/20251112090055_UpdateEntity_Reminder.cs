using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntity_Reminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TriggeredAt",
                schema: "Core",
                table: "UserReminders");

            migrationBuilder.RenameColumn(
                name: "TriggerAt",
                schema: "Core",
                table: "UserReminders",
                newName: "ResolvedAt");

            migrationBuilder.RenameColumn(
                name: "IsTriggered",
                schema: "Core",
                table: "UserReminders",
                newName: "IsResolved");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                schema: "Core",
                table: "UserReminders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DueAt",
                schema: "Core",
                table: "UserReminders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SlaHours",
                schema: "Core",
                table: "UserReminders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                schema: "Core",
                table: "UserReminders");

            migrationBuilder.DropColumn(
                name: "DueAt",
                schema: "Core",
                table: "UserReminders");

            migrationBuilder.DropColumn(
                name: "SlaHours",
                schema: "Core",
                table: "UserReminders");

            migrationBuilder.RenameColumn(
                name: "ResolvedAt",
                schema: "Core",
                table: "UserReminders",
                newName: "TriggerAt");

            migrationBuilder.RenameColumn(
                name: "IsResolved",
                schema: "Core",
                table: "UserReminders",
                newName: "IsTriggered");

            migrationBuilder.AddColumn<DateTime>(
                name: "TriggeredAt",
                schema: "Core",
                table: "UserReminders",
                type: "datetime2",
                nullable: true);
        }
    }
}
