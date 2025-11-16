using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBudgetCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetPlan_BudgetCode_BudgetCodeId",
                schema: "Finance",
                table: "BudgetPlan");

            migrationBuilder.DropIndex(
                name: "IX_BudgetPlan_BudgetCodeId",
                schema: "Finance",
                table: "BudgetPlan");

            migrationBuilder.DropColumn(
                name: "BudgetCodeId",
                schema: "Finance",
                table: "BudgetPlan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BudgetCodeId",
                schema: "Finance",
                table: "BudgetPlan",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlan_BudgetCodeId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "BudgetCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlan_BudgetCode_BudgetCodeId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "BudgetCodeId",
                principalSchema: "Finance",
                principalTable: "BudgetCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
