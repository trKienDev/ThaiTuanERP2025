using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEnittyExpenseWorkflowInstance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance");

            migrationBuilder.DropColumn(
                name: "BudgetCode",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance");

            migrationBuilder.DropColumn(
                name: "CostCenter",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance");

            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance");

            migrationBuilder.DropColumn(
                name: "RawJson",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentType",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DocumentType",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedBy",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BudgetCode",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostCenter",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RawJson",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
