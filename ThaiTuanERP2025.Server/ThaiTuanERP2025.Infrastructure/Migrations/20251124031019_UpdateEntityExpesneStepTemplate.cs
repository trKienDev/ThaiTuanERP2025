using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityExpesneStepTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseStepTemplates_Users_CreatedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseStepTemplates_Users_DeletedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseStepTemplates_Users_ModifiedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseStepTemplates_CreatedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseStepTemplates_DeletedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseStepTemplates_ModifiedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.DropColumn(
                name: "AllowOverride",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.RenameColumn(
                name: "ApproverMode",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                newName: "ExpenseApproveMode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpenseApproveMode",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                newName: "ApproverMode");

            migrationBuilder.AddColumn<bool>(
                name: "AllowOverride",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepTemplates_CreatedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepTemplates_DeletedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepTemplates_ModifiedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                column: "ModifiedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepTemplates_Users_CreatedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepTemplates_Users_DeletedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepTemplates_Users_ModifiedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
