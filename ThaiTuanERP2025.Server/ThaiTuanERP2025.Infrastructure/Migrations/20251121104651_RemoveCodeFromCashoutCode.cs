using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCodeFromCashoutCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CashoutCodes_CashoutGroupId_Code",
                schema: "Finance",
                table: "CashoutCodes");

            migrationBuilder.DropIndex(
                name: "IX_CashoutCodes_Name",
                schema: "Finance",
                table: "CashoutCodes");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Finance",
                table: "CashoutCodes");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutCodes_CashoutGroupId",
                schema: "Finance",
                table: "CashoutCodes",
                column: "CashoutGroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashoutCodes_Name",
                schema: "Finance",
                table: "CashoutCodes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CashoutCodes_CashoutGroupId",
                schema: "Finance",
                table: "CashoutCodes");

            migrationBuilder.DropIndex(
                name: "IX_CashoutCodes_Name",
                schema: "Finance",
                table: "CashoutCodes");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Finance",
                table: "CashoutCodes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutCodes_CashoutGroupId_Code",
                schema: "Finance",
                table: "CashoutCodes",
                columns: new[] { "CashoutGroupId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashoutCodes_Name",
                schema: "Finance",
                table: "CashoutCodes",
                column: "Name");
        }
    }
}
