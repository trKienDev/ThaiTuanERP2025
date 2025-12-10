using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEntityBudgetTransaction : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// === 1. DROP FK CŨ ===
			migrationBuilder.DropForeignKey(
			    name: "FK_BudgetTransaction_BudgetPlan_BudgetPlanId",
			    schema: "Finance",
			    table: "BudgetTransaction");


			// === 2. RENAME CÁC COLUMN CŨ (GIỮ DỮ LIỆU) ===
			migrationBuilder.RenameColumn(
			    name: "BudgetPlanId",
			    schema: "Finance",
			    table: "BudgetTransaction",
			    newName: "OldBudgetPlanId");

			migrationBuilder.RenameColumn(
			    name: "PaymentId",
			    schema: "Finance",
			    table: "BudgetTransaction",
			    newName: "OldPaymentId");


			// === 3. THÊM COLUMN MỚI (NULLABLE ĐỂ TRÁNH LỖI) ===
			migrationBuilder.AddColumn<Guid>(
			    name: "BudgetPlanDetailId",
			    schema: "Finance",
			    table: "BudgetTransaction",
			    type: "uniqueidentifier",
			    nullable: true);

			migrationBuilder.AddColumn<Guid>(
			    name: "ExpensePaymentItemId",
			    schema: "Finance",
			    table: "BudgetTransaction",
			    type: "uniqueidentifier",
			    nullable: true);


			// === 4. THÊM FK MỚI ===

			// FK → BudgetPlanDetail
			migrationBuilder.AddForeignKey(
			    name: "FK_BudgetTransaction_BudgetPlanDetail_BudgetPlanDetailId",
			    schema: "Finance",
			    table: "BudgetTransaction",
			    column: "BudgetPlanDetailId",
			    principalSchema: "Finance",
			    principalTable: "BudgetPlanDetails",   // tên bảng thực tế của bạn
			    principalColumn: "Id",
			    onDelete: ReferentialAction.Restrict
			);

			// FK → ExpensePaymentItem
			migrationBuilder.AddForeignKey(
			    name: "FK_BudgetTransaction_ExpensePaymentItems_ExpensePaymentItemId",
			    schema: "Finance",
			    table: "BudgetTransaction",
			    column: "ExpensePaymentItemId",
			    principalSchema: "Expense",
			    principalTable: "ExpensePaymentItems",
			    principalColumn: "Id",
			    onDelete: ReferentialAction.Restrict
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// === 1. XOÁ FK MỚI ===
			migrationBuilder.DropForeignKey(
			    name: "FK_BudgetTransaction_BudgetPlanDetail_BudgetPlanDetailId",
			    schema: "Finance",
			    table: "BudgetTransaction");

			migrationBuilder.DropForeignKey(
			    name: "FK_BudgetTransaction_ExpensePaymentItems_ExpensePaymentItemId",
			    schema: "Finance",
			    table: "BudgetTransaction");

			// === 2. XOÁ COLUMN MỚI ===
			migrationBuilder.DropColumn(
			    name: "BudgetPlanDetailId",
			    schema: "Finance",
			    table: "BudgetTransaction");

			migrationBuilder.DropColumn(
			    name: "ExpensePaymentItemId",
			    schema: "Finance",
			    table: "BudgetTransaction");

			// === 3. ĐỔI TÊN COLUMN VỀ LẠI ===
			migrationBuilder.RenameColumn(
			    name: "OldBudgetPlanId",
			    schema: "Finance",
			    table: "BudgetTransaction",
			    newName: "BudgetPlanId");

			migrationBuilder.RenameColumn(
			    name: "OldPaymentId",
			    schema: "Finance",
			    table: "BudgetTransaction",
			    newName: "PaymentId");


			// === 4. KHÔI PHỤC FK CŨ (OPTIONAL) ===
			migrationBuilder.AddForeignKey(
			    name: "FK_BudgetTransaction_BudgetPlan_BudgetPlanId",
			    schema: "Finance",
			    table: "BudgetTransaction",
			    column: "BudgetPlanId",
			    principalSchema: "Finance",
			    principalTable: "BudgetPlan",
			    principalColumn: "Id",
			    onDelete: ReferentialAction.Cascade
			);
		}
	}
}
