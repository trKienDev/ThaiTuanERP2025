using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsActiveFromBudgetPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BudgetPeriod_IsActive",
                schema: "Finance",
                table: "BudgetPeriod");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Finance",
                table: "BudgetPeriod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Finance",
                table: "BudgetPeriod",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPeriod_IsActive",
                schema: "Finance",
                table: "BudgetPeriod",
                column: "IsActive");
        }
    }
}
