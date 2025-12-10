using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameDueAtInExpensePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExpensePayments_DueDate",
                schema: "Expense",
                table: "ExpensePayments");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                schema: "Expense",
                table: "ExpensePayments",
                newName: "DueAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DueAt",
                schema: "Expense",
                table: "ExpensePayments",
                newName: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePayments_DueDate",
                schema: "Expense",
                table: "ExpensePayments",
                column: "DueDate");
        }
    }
}
