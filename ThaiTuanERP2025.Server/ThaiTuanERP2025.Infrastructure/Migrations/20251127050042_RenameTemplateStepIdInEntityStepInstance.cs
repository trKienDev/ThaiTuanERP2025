using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTemplateStepIdInEntityStepInstance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseStepInstances_ExpenseStepTemplates_TemplateStepId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseStepInstances_TemplateStepId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances");

            migrationBuilder.DropColumn(
                name: "TemplateStepId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances");

            migrationBuilder.AddColumn<Guid>(
                name: "StepTemplateId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepInstances_StepTemplateId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "StepTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepInstances_ExpenseStepTemplates_StepTemplateId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "StepTemplateId",
                principalSchema: "ExpenseWorkflow",
                principalTable: "ExpenseStepTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseStepInstances_ExpenseStepTemplates_StepTemplateId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseStepInstances_StepTemplateId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances");

            migrationBuilder.DropColumn(
                name: "StepTemplateId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances");

            migrationBuilder.AddColumn<Guid>(
                name: "TemplateStepId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepInstances_TemplateStepId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "TemplateStepId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepInstances_ExpenseStepTemplates_TemplateStepId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "TemplateStepId",
                principalSchema: "ExpenseWorkflow",
                principalTable: "ExpenseStepTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
