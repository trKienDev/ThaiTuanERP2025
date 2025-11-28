using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDefaultApproverAndSelectedApproverOfStepInstance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultApproverId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances");

            migrationBuilder.DropColumn(
                name: "HistoryJson",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances");

            migrationBuilder.DropColumn(
                name: "ResolvedApproverCandidatesJson",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances");

            migrationBuilder.DropColumn(
                name: "SelectedApproverId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances");

            migrationBuilder.AddColumn<string>(
                name: "ResolvedApproversJson",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResolvedApproversJson",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances");

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultApproverId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HistoryJson",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolvedApproverCandidatesJson",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SelectedApproverId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
