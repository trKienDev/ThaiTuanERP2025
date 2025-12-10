using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReversedToBudgetTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OriginalTransactionId",
                schema: "Finance",
                table: "BudgetTransaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReversedByTransactionId",
                schema: "Finance",
                table: "BudgetTransaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTransaction_OriginalTransactionId",
                schema: "Finance",
                table: "BudgetTransaction",
                column: "OriginalTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTransaction_ReversedByTransactionId",
                schema: "Finance",
                table: "BudgetTransaction",
                column: "ReversedByTransactionId",
                unique: true,
                filter: "[ReversedByTransactionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetTransaction_BudgetTransaction_ReversedByTransactionId",
                schema: "Finance",
                table: "BudgetTransaction",
                column: "ReversedByTransactionId",
                principalSchema: "Finance",
                principalTable: "BudgetTransaction",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetTransaction_BudgetTransaction_ReversedByTransactionId",
                schema: "Finance",
                table: "BudgetTransaction");

            migrationBuilder.DropIndex(
                name: "IX_BudgetTransaction_OriginalTransactionId",
                schema: "Finance",
                table: "BudgetTransaction");

            migrationBuilder.DropIndex(
                name: "IX_BudgetTransaction_ReversedByTransactionId",
                schema: "Finance",
                table: "BudgetTransaction");

            migrationBuilder.DropColumn(
                name: "OriginalTransactionId",
                schema: "Finance",
                table: "BudgetTransaction");

            migrationBuilder.DropColumn(
                name: "ReversedByTransactionId",
                schema: "Finance",
                table: "BudgetTransaction");
        }
    }
}
