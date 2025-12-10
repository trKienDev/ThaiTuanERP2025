using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBudgetPlanDetail_AllowNullableModifiedAt : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<Guid>(
			    name: "ModifiedByUserId",
			    schema: "Finance",
			    table: "BudgetPlanDetails",
			    type: "uniqueidentifier",
			    nullable: true,
			    oldClrType: typeof(Guid),
			    oldType: "uniqueidentifier");

			migrationBuilder.AlterColumn<DateTime>(
			    name: "ModifiedAt",
			    schema: "Finance",
			    table: "BudgetPlanDetails",
			    type: "datetime2",
			    nullable: true,
			    oldClrType: typeof(DateTime),
			    oldType: "datetime2");

			migrationBuilder.AlterColumn<Guid>(
			    name: "DeletedByUserId",
			    schema: "Finance",
			    table: "BudgetPlanDetails",
			    type: "uniqueidentifier",
			    nullable: true,
			    oldClrType: typeof(Guid),
			    oldType: "uniqueidentifier");

			migrationBuilder.AlterColumn<DateTime>(
			    name: "DeletedAt",
			    schema: "Finance",
			    table: "BudgetPlanDetails",
			    type: "datetime2",
			    nullable: true,
			    oldClrType: typeof(DateTime),
			    oldType: "datetime2");
		}


		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<Guid>(
			    name: "ModifiedByUserId",
			    schema: "Finance",
			    table: "BudgetPlanDetails",
			    type: "uniqueidentifier",
			    nullable: false,
			    oldClrType: typeof(Guid),
			    oldType: "uniqueidentifier",
			    oldNullable: true);

			migrationBuilder.AlterColumn<Guid>(
			name: "ModifiedAt",
			schema: "Finance",
			table: "BudgetPlanDetails",
			type: "uniqueidentifier",
			nullable: false,
			oldClrType: typeof(Guid),
			oldType: "uniqueidentifier",
			oldNullable: true);

			// tương tự cho ModifiedAt, DeletedAt, DeletedByUserId
		}

	}
}
