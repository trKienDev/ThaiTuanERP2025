using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReceiverAndSenderToUserNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_Users_UserId",
                schema: "Core",
                table: "UserNotifications");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "Core",
                table: "UserNotifications",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotifications_UserId",
                schema: "Core",
                table: "UserNotifications",
                newName: "IX_UserNotifications_SenderId");

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiverId",
                schema: "Core",
                table: "UserNotifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_ReceiverId",
                schema: "Core",
                table: "UserNotifications",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_Users_ReceiverId",
                schema: "Core",
                table: "UserNotifications",
                column: "ReceiverId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_Users_SenderId",
                schema: "Core",
                table: "UserNotifications",
                column: "SenderId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_Users_ReceiverId",
                schema: "Core",
                table: "UserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_Users_SenderId",
                schema: "Core",
                table: "UserNotifications");

            migrationBuilder.DropIndex(
                name: "IX_UserNotifications_ReceiverId",
                schema: "Core",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                schema: "Core",
                table: "UserNotifications");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                schema: "Core",
                table: "UserNotifications",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotifications_SenderId",
                schema: "Core",
                table: "UserNotifications",
                newName: "IX_UserNotifications_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_Users_UserId",
                schema: "Core",
                table: "UserNotifications",
                column: "UserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
