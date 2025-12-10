using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCodeFromCashoutGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CashoutGroups_Code",
                schema: "Finance",
                table: "CashoutGroups");

            migrationBuilder.DropIndex(
                name: "IX_CashoutGroups_Name",
                schema: "Finance",
                table: "CashoutGroups");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Finance",
                table: "CashoutGroups");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutGroups_Name",
                schema: "Finance",
                table: "CashoutGroups",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LedgerAccounts_Number_IsDeleted",
                schema: "Finance",
                table: "LedgerAccounts");

            migrationBuilder.DropIndex(
                name: "IX_LedgerAccounts_Path",
                schema: "Finance",
                table: "LedgerAccounts");

            migrationBuilder.DropIndex(
                name: "IX_CashoutGroups_Name",
                schema: "Finance",
                table: "CashoutGroups");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Finance",
                table: "CashoutGroups",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutGroups_Code",
                schema: "Finance",
                table: "CashoutGroups",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashoutGroups_Name",
                schema: "Finance",
                table: "CashoutGroups",
                column: "Name");
        }
    }
}
