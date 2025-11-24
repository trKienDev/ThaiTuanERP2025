using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDocumentTypeFromExpenseWorkflowTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExpenseWorkflowTemplates_DocumentType_Version",
                schema: "ExpenseWorkflow",
                table: "ExpenseWorkflowTemplates");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                schema: "ExpenseWorkflow",
                table: "ExpenseWorkflowTemplates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentType",
                schema: "ExpenseWorkflow",
                table: "ExpenseWorkflowTemplates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseWorkflowTemplates_DocumentType_Version",
                schema: "ExpenseWorkflow",
                table: "ExpenseWorkflowTemplates",
                columns: new[] { "DocumentType", "Version" });
        }
    }
}
