using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameApproveModeInExpenseStepTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseApproveMode",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.AddColumn<byte>(
                name: "ApproveMode",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveMode",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseApproveMode",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
