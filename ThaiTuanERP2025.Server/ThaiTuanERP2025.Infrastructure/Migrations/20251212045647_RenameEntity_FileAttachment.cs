using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameEntity_FileAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FileAttachment_Bucket_ObjectKey",
                schema: "Core",
                table: "FileAttachment");

            migrationBuilder.DropColumn(
                name: "Bucket",
                schema: "Core",
                table: "FileAttachment");

            migrationBuilder.DropColumn(
                name: "Hash",
                schema: "Core",
                table: "FileAttachment");

            migrationBuilder.DropColumn(
                name: "ObjectKey",
                schema: "Core",
                table: "FileAttachment");

            migrationBuilder.AddColumn<Guid>(
                name: "DriveObjectId",
                schema: "Core",
                table: "FileAttachment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachment_DriveObjectId",
                schema: "Core",
                table: "FileAttachment",
                column: "DriveObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FileAttachment_DriveObjectId",
                schema: "Core",
                table: "FileAttachment");

            migrationBuilder.DropColumn(
                name: "DriveObjectId",
                schema: "Core",
                table: "FileAttachment");

            migrationBuilder.AddColumn<string>(
                name: "Bucket",
                schema: "Core",
                table: "FileAttachment",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                schema: "Core",
                table: "FileAttachment",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectKey",
                schema: "Core",
                table: "FileAttachment",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachment_Bucket_ObjectKey",
                schema: "Core",
                table: "FileAttachment",
                columns: new[] { "Bucket", "ObjectKey" },
                unique: true);
        }
    }
}
