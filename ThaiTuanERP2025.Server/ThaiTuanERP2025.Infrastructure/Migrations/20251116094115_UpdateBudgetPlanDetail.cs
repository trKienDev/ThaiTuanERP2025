using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBudgetPlanDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetPlanDetails_Users_DeletedByUserId",
                schema: "Finance",
                table: "BudgetPlanDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetPlanDetails_Users_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetPlanDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlanDetails_Users_DeletedByUserId",
                schema: "Finance",
                table: "BudgetPlanDetails",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlanDetails_Users_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetPlanDetails",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetPlanDetails_Users_DeletedByUserId",
                schema: "Finance",
                table: "BudgetPlanDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetPlanDetails_Users_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetPlanDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlanDetails_Users_DeletedByUserId",
                schema: "Finance",
                table: "BudgetPlanDetails",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlanDetails_Users_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetPlanDetails",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
