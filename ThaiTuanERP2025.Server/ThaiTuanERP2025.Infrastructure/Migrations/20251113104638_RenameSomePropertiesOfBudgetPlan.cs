using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameSomePropertiesOfBudgetPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedBudgetApproverId",
                schema: "Finance",
                table: "BudgetPlan");

            migrationBuilder.DropColumn(
                name: "SelectedReviewerUserId",
                schema: "Finance",
                table: "BudgetPlan");

            migrationBuilder.AddColumn<Guid>(
                name: "SelectedApproverId",
                schema: "Finance",
                table: "BudgetPlan",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SelectedReviewerId",
                schema: "Finance",
                table: "BudgetPlan",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedApproverId",
                schema: "Finance",
                table: "BudgetPlan");

            migrationBuilder.DropColumn(
                name: "SelectedReviewerId",
                schema: "Finance",
                table: "BudgetPlan");

            migrationBuilder.AddColumn<Guid>(
                name: "SelectedBudgetApproverId",
                schema: "Finance",
                table: "BudgetPlan",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SelectedReviewerUserId",
                schema: "Finance",
                table: "BudgetPlan",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
