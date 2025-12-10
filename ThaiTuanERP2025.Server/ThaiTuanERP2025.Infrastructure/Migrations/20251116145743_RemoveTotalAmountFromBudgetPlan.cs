using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTotalAmountFromBudgetPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                schema: "Finance",
                table: "BudgetPlan");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlanDetails_BudgetCode_BudgetCodeId",
                schema: "Finance",
                table: "BudgetPlanDetails",
                column: "BudgetCodeId",
                principalSchema: "Finance",
                principalTable: "BudgetCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetPlanDetails_BudgetCode_BudgetCodeId",
                schema: "Finance",
                table: "BudgetPlanDetails");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                schema: "Finance",
                table: "BudgetPlan",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
