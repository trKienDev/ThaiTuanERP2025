using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAuditableEntityFollower : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_CreatedByUserId",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_DeletedByUserId",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_ModifiedByUserId",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Followers",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropIndex(
                name: "IX_Followers_CreatedByUserId",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropIndex(
                name: "IX_Followers_DeletedByUserId",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropIndex(
                name: "IX_Followers_ModifiedByUserId",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                schema: "Core",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                schema: "Core",
                table: "Followers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Followers",
                schema: "Core",
                table: "Followers",
                columns: new[] { "DocumentId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Followers",
                schema: "Core",
                table: "Followers");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "Core",
                table: "Followers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "Core",
                table: "Followers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                schema: "Core",
                table: "Followers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "Core",
                table: "Followers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedByUserId",
                schema: "Core",
                table: "Followers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Core",
                table: "Followers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Core",
                table: "Followers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                schema: "Core",
                table: "Followers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                schema: "Core",
                table: "Followers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Followers",
                schema: "Core",
                table: "Followers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_CreatedByUserId",
                schema: "Core",
                table: "Followers",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_DeletedByUserId",
                schema: "Core",
                table: "Followers",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_ModifiedByUserId",
                schema: "Core",
                table: "Followers",
                column: "ModifiedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_CreatedByUserId",
                schema: "Core",
                table: "Followers",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_DeletedByUserId",
                schema: "Core",
                table: "Followers",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_ModifiedByUserId",
                schema: "Core",
                table: "Followers",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
