using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameDueAt_PostingAt_PaymentAt_Entity_OutgoingPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostingDate",
                schema: "Expense",
                table: "OutgoingPayments",
                newName: "PostingAt");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                schema: "Expense",
                table: "OutgoingPayments",
                newName: "PaymentAt");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                schema: "Expense",
                table: "OutgoingPayments",
                newName: "DueAt");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingPayments_PostingDate",
                schema: "Expense",
                table: "OutgoingPayments",
                newName: "IX_OutgoingPayments_PostingAt");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingPayments_PaymentDate",
                schema: "Expense",
                table: "OutgoingPayments",
                newName: "IX_OutgoingPayments_PaymentAt");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingPayments_DueDate",
                schema: "Expense",
                table: "OutgoingPayments",
                newName: "IX_OutgoingPayments_DueAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostingAt",
                schema: "Expense",
                table: "OutgoingPayments",
                newName: "PostingDate");

            migrationBuilder.RenameColumn(
                name: "PaymentAt",
                schema: "Expense",
                table: "OutgoingPayments",
                newName: "PaymentDate");

            migrationBuilder.RenameColumn(
                name: "DueAt",
                schema: "Expense",
                table: "OutgoingPayments",
                newName: "DueDate");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingPayments_PostingAt",
                schema: "Expense",
                table: "OutgoingPayments",
                newName: "IX_OutgoingPayments_PostingDate");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingPayments_PaymentAt",
                schema: "Expense",
                table: "OutgoingPayments",
                newName: "IX_OutgoingPayments_PaymentDate");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingPayments_DueAt",
                schema: "Expense",
                table: "OutgoingPayments",
                newName: "IX_OutgoingPayments_DueDate");
        }
    }
}
