using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IndexNameOfExpensePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ExpensePayments_Name",
                schema: "Expense",
                table: "ExpensePayments",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExpensePayments_Name",
                schema: "Expense",
                table: "ExpensePayments");
        }
    }
}
