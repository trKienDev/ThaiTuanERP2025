using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBeneficiaryInfor_Supplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryAccountNumber",
                schema: "Expense",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryBankName",
                schema: "Expense",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryName",
                schema: "Expense",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeneficiaryAccountNumber",
                schema: "Expense",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "BeneficiaryBankName",
                schema: "Expense",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "BeneficiaryName",
                schema: "Expense",
                table: "Suppliers");
        }
    }
}
