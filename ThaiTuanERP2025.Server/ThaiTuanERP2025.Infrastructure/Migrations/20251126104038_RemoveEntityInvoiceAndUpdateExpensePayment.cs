using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEntityInvoiceAndUpdateExpensePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePaymentItems_BudgetCode_BudgetCodeId",
                schema: "Expense",
                table: "ExpensePaymentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePaymentItems_CashoutCodes_CashoutCodeId",
                schema: "Expense",
                table: "ExpensePaymentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePaymentItems_Invoices_InvoiceId",
                schema: "Expense",
                table: "ExpensePaymentItems");

            migrationBuilder.DropTable(
                name: "InvoiceFiles",
                schema: "Expense");

            migrationBuilder.DropTable(
                name: "Invoices",
                schema: "Expense");

            migrationBuilder.DropIndex(
                name: "IX_ExpensePaymentItems_BudgetCodeId",
                schema: "Expense",
                table: "ExpensePaymentItems");

            migrationBuilder.DropIndex(
                name: "IX_ExpensePaymentItems_CashoutCodeId",
                schema: "Expense",
                table: "ExpensePaymentItems");

            migrationBuilder.DropColumn(
                name: "BudgetCodeId",
                schema: "Expense",
                table: "ExpensePaymentItems");

            migrationBuilder.DropColumn(
                name: "CashoutCodeId",
                schema: "Expense",
                table: "ExpensePaymentItems");

            migrationBuilder.RenameColumn(
                name: "InvoiceId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                newName: "InvoiceFileId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpensePaymentItems_InvoiceId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                newName: "IX_ExpensePaymentItems_InvoiceFileId");

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetPlanDetailId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentItems_BudgetPlanDetailId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "BudgetPlanDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentItems_BudgetPlanDetails_BudgetPlanDetailId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "BudgetPlanDetailId",
                principalSchema: "Finance",
                principalTable: "BudgetPlanDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentItems_StoredFiles_InvoiceFileId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "InvoiceFileId",
                principalSchema: "Files",
                principalTable: "StoredFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePaymentItems_BudgetPlanDetails_BudgetPlanDetailId",
                schema: "Expense",
                table: "ExpensePaymentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePaymentItems_StoredFiles_InvoiceFileId",
                schema: "Expense",
                table: "ExpensePaymentItems");

            migrationBuilder.DropIndex(
                name: "IX_ExpensePaymentItems_BudgetPlanDetailId",
                schema: "Expense",
                table: "ExpensePaymentItems");

            migrationBuilder.DropColumn(
                name: "BudgetPlanDetailId",
                schema: "Expense",
                table: "ExpensePaymentItems");

            migrationBuilder.RenameColumn(
                name: "InvoiceFileId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                newName: "InvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpensePaymentItems_InvoiceFileId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                newName: "IX_ExpensePaymentItems_InvoiceId");

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetCodeId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CashoutCodeId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Invoices",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BuyerAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    BuyerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BuyerTaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoiceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SellerAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    SellerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SellerTaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalWithTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Invoices_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceFiles",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceFiles_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "Expense",
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceFiles_StoredFiles_FileId",
                        column: x => x.FileId,
                        principalSchema: "Files",
                        principalTable: "StoredFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceFiles_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceFiles_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceFiles_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentItems_BudgetCodeId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "BudgetCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentItems_CashoutCodeId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "CashoutCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceFiles_CreatedAt",
                schema: "Expense",
                table: "InvoiceFiles",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceFiles_CreatedByUserId",
                schema: "Expense",
                table: "InvoiceFiles",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceFiles_DeletedByUserId",
                schema: "Expense",
                table: "InvoiceFiles",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceFiles_FileId",
                schema: "Expense",
                table: "InvoiceFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceFiles_InvoiceId_IsMain",
                schema: "Expense",
                table: "InvoiceFiles",
                columns: new[] { "InvoiceId", "IsMain" },
                unique: true,
                filter: "[IsMain] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceFiles_ModifiedByUserId",
                schema: "Expense",
                table: "InvoiceFiles",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CreatedByUserId",
                schema: "Expense",
                table: "Invoices",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_DeletedByUserId",
                schema: "Expense",
                table: "Invoices",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceNumber",
                schema: "Expense",
                table: "Invoices",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_IssueDate",
                schema: "Expense",
                table: "Invoices",
                column: "IssueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ModifiedByUserId",
                schema: "Expense",
                table: "Invoices",
                column: "ModifiedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentItems_BudgetCode_BudgetCodeId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "BudgetCodeId",
                principalSchema: "Finance",
                principalTable: "BudgetCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentItems_CashoutCodes_CashoutCodeId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "CashoutCodeId",
                principalSchema: "Finance",
                principalTable: "CashoutCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentItems_Invoices_InvoiceId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "InvoiceId",
                principalSchema: "Expense",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
