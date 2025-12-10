using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReDesignEntity_ExpensePaymentAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePaymentAttachments_Users_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePaymentAttachments");

            migrationBuilder.DropIndex(
                name: "IX_ExpensePaymentAttachments_FileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments");

            migrationBuilder.DropColumn(
                name: "FileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments");

            migrationBuilder.DropColumn(
                name: "FileName",
                schema: "Expense",
                table: "ExpensePaymentAttachments");

            migrationBuilder.DropColumn(
                name: "ObjectKey",
                schema: "Expense",
                table: "ExpensePaymentAttachments");

            migrationBuilder.DropColumn(
                name: "Size",
                schema: "Expense",
                table: "ExpensePaymentAttachments");

            migrationBuilder.DropColumn(
                name: "Url",
                schema: "Expense",
                table: "ExpensePaymentAttachments");

            migrationBuilder.AddColumn<Guid>(
                name: "StoredFileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentAttachments_StoredFileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "StoredFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentAttachments_StoredFiles_StoredFileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "StoredFileId",
                principalSchema: "Files",
                principalTable: "StoredFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentAttachments_Users_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePaymentAttachments_StoredFiles_StoredFileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePaymentAttachments_Users_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePaymentAttachments");

            migrationBuilder.DropIndex(
                name: "IX_ExpensePaymentAttachments_StoredFileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments");

            migrationBuilder.DropColumn(
                name: "StoredFileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments");

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ObjectKey",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentAttachments_FileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentAttachments_Users_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
