using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntity_OutgoingBankAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OutgoingBankAccounts_AccountNumber",
                schema: "Expense",
                table: "OutgoingBankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_OutgoingBankAccounts_IsActive_Name",
                schema: "Expense",
                table: "OutgoingBankAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingBankAccounts_Name",
                schema: "Expense",
                table: "OutgoingBankAccounts",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OutgoingBankAccounts_Name",
                schema: "Expense",
                table: "OutgoingBankAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingBankAccounts_AccountNumber",
                schema: "Expense",
                table: "OutgoingBankAccounts",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingBankAccounts_IsActive_Name",
                schema: "Expense",
                table: "OutgoingBankAccounts",
                columns: new[] { "IsActive", "Name" });
        }
    }
}
