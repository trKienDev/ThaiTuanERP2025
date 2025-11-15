using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Finance");

            migrationBuilder.EnsureSchema(
                name: "Account");

            migrationBuilder.EnsureSchema(
                name: "Expense");

            migrationBuilder.EnsureSchema(
                name: "ExpenseWorkflow");

            migrationBuilder.EnsureSchema(
                name: "Core");

            migrationBuilder.EnsureSchema(
                name: "RBAC");

            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.EnsureSchema(
                name: "Files");

            migrationBuilder.CreateTable(
                name: "ExpesneWorkflowInstance",
                schema: "ExpenseWorkflow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateVersion = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CurrentStepOrder = table.Column<int>(type: "int", nullable: false),
                    RawJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BudgetCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CostCenter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpesneWorkflowInstance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NumberSeries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PadLength = table.Column<int>(type: "int", nullable: false, defaultValue: 6),
                    NextNumber = table.Column<long>(type: "bigint", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberSeries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ReplacedByTokenHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "RBAC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetApprover",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApproverUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SlaHours = table.Column<int>(type: "int", nullable: false, defaultValue: 8),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetApprover", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetApproverDepartments",
                schema: "Finance",
                columns: table => new
                {
                    BudgetApproverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetApproverDepartments", x => new { x.BudgetApproverId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_BudgetApproverDepartments_BudgetApprover_BudgetApproverId",
                        column: x => x.BudgetApproverId,
                        principalSchema: "Finance",
                        principalTable: "BudgetApprover",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetCode",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BudgetGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CashoutCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetGroup",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetPeriod",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetPeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetPlan",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BudgetPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DueAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IsReviewed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SelectedReviewerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SelectedApproverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BudgetApproverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BudgetCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetPlan_BudgetApprover_BudgetApproverId",
                        column: x => x.BudgetApproverId,
                        principalSchema: "Finance",
                        principalTable: "BudgetApprover",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BudgetPlan_BudgetCode_BudgetCodeId",
                        column: x => x.BudgetCodeId,
                        principalSchema: "Finance",
                        principalTable: "BudgetCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetPlan_BudgetPeriod_BudgetPeriodId",
                        column: x => x.BudgetPeriodId,
                        principalSchema: "Finance",
                        principalTable: "BudgetPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetTransaction",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BudgetPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetTransaction_BudgetPlan_BudgetPlanId",
                        column: x => x.BudgetPlanId,
                        principalSchema: "Finance",
                        principalTable: "BudgetPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetPlanDetails",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BudgetPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BudgetCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetPlanDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetPlanDetails_BudgetPlan_BudgetPlanId",
                        column: x => x.BudgetPlanId,
                        principalSchema: "Finance",
                        principalTable: "BudgetPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashoutCodes",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CashoutGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostingLedgerAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LedgerAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashoutCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashoutGroups",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashoutGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashoutGroups_CashoutGroups_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Finance",
                        principalTable: "CashoutGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentManagers",
                schema: "Account",
                columns: table => new
                {
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentManagers", x => new { x.DepartmentId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Level = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Account",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentSequences",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastNumber = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentSequences", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "ExpensePaymentAttachments",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpensePaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObjectKey = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensePaymentAttachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpensePaymentCommentAttachments",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensePaymentCommentAttachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpensePaymentComments",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpensePaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    CommentType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensePaymentComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpensePaymentComments_ExpensePaymentComments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalSchema: "Expense",
                        principalTable: "ExpensePaymentComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpensePaymentCommentTags",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensePaymentCommentTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpensePaymentCommentTags_ExpensePaymentComments_CommentId",
                        column: x => x.CommentId,
                        principalSchema: "Expense",
                        principalTable: "ExpensePaymentComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpensePaymentItems",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpensePaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxRate = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalWithTax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BudgetCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CashoutCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensePaymentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpensePaymentItems_BudgetCode_BudgetCodeId",
                        column: x => x.BudgetCodeId,
                        principalSchema: "Finance",
                        principalTable: "BudgetCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpensePaymentItems_CashoutCodes_CashoutCodeId",
                        column: x => x.CashoutCodeId,
                        principalSchema: "Finance",
                        principalTable: "CashoutCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpensePayments",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SubId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    PayeeType = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    BeneficiaryName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasGoodsReceipt = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalWithTax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OutgoingAmountPaid = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    RemainingOutgoingAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CurrentWorkflowInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ManagerApproverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpensePayments_ExpesneWorkflowInstance_CurrentWorkflowInstanceId",
                        column: x => x.CurrentWorkflowInstanceId,
                        principalSchema: "ExpenseWorkflow",
                        principalTable: "ExpesneWorkflowInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseStepInstances",
                schema: "ExpenseWorkflow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkflowInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    FlowType = table.Column<int>(type: "int", nullable: false),
                    SlaHours = table.Column<int>(type: "int", nullable: false),
                    ApproverMode = table.Column<int>(type: "int", nullable: false),
                    ResolvedApproverCandidatesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultApproverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SelectedApproverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlaBreached = table.Column<bool>(type: "bit", nullable: false),
                    HistoryJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseStepInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseStepInstances_ExpesneWorkflowInstance_WorkflowInstanceId",
                        column: x => x.WorkflowInstanceId,
                        principalSchema: "ExpenseWorkflow",
                        principalTable: "ExpesneWorkflowInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseStepTemplates",
                schema: "ExpenseWorkflow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkflowTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    FlowType = table.Column<int>(type: "int", nullable: false),
                    SlaHours = table.Column<int>(type: "int", nullable: false),
                    ApproverMode = table.Column<int>(type: "int", nullable: false),
                    FixedApproverIdsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolverKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResolverParamsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowOverride = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseStepTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseWorkflowTemplates",
                schema: "ExpenseWorkflow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseWorkflowTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Followers",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceFiles",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvoiceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SellerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SellerTaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SellerAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    BuyerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BuyerTaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BuyerAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalWithTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LedgerAccounts",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LedgerAccountTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentLedgerAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LedgerAccountBalanceType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LedgerAccounts_LedgerAccounts_ParentLedgerAccountId",
                        column: x => x.ParentLedgerAccountId,
                        principalSchema: "Finance",
                        principalTable: "LedgerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LedgerAccountTypes",
                schema: "Finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LedgerAccountTypeKind = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerAccountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingBankAccounts",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingBankAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingPayments",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SubId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    OutgoingAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 30, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    BeneficiaryName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PostingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OutgoingBankAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpensePaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutgoingPayments_ExpensePayments_ExpensePaymentId",
                        column: x => x.ExpensePaymentId,
                        principalSchema: "Expense",
                        principalTable: "ExpensePayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutgoingPayments_OutgoingBankAccounts_OutgoingBankAccountId",
                        column: x => x.OutgoingBankAccountId,
                        principalSchema: "Expense",
                        principalTable: "OutgoingBankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "RBAC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                schema: "RBAC",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "RBAC",
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "RBAC",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoredFiles",
                schema: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Bucket = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ObjectKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Module = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Entity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AvatarFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AvatarFileObjectKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsSuperAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    OutgoingPaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Account",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_OutgoingPayments_OutgoingPaymentId",
                        column: x => x.OutgoingPaymentId,
                        principalSchema: "Expense",
                        principalTable: "OutgoingPayments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_StoredFiles_AvatarFileId",
                        column: x => x.AvatarFileId,
                        principalSchema: "Files",
                        principalTable: "StoredFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suppliers_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Suppliers_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Suppliers_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeftAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Account",
                        principalTable: "Groups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserManagerAssignments",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserManagerAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserManagerAssignments_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserManagerAssignments_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserManagerAssignments_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserManagerAssignments_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserManagerAssignments_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LinkType = table.Column<int>(type: "int", nullable: false),
                    TargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNotifications_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserNotifications_Users_SenderId",
                        column: x => x.SenderId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserReminders",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SlaHours = table.Column<int>(type: "int", nullable: false),
                    DueAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReminders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserReminders_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "RBAC",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "RBAC",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetApprover_ApproverUserId",
                schema: "Finance",
                table: "BudgetApprover",
                column: "ApproverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetApprover_CreatedByUserId",
                schema: "Finance",
                table: "BudgetApprover",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetApprover_DeletedByUserId",
                schema: "Finance",
                table: "BudgetApprover",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetApprover_IsActive",
                schema: "Finance",
                table: "BudgetApprover",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetApprover_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetApprover",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetApproverDepartments_DepartmentId",
                schema: "Finance",
                table: "BudgetApproverDepartments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCode_BudgetGroupId_Code",
                schema: "Finance",
                table: "BudgetCode",
                columns: new[] { "BudgetGroupId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCode_CashoutCodeId",
                schema: "Finance",
                table: "BudgetCode",
                column: "CashoutCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCode_CreatedByUserId",
                schema: "Finance",
                table: "BudgetCode",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCode_DeletedByUserId",
                schema: "Finance",
                table: "BudgetCode",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCode_Id",
                schema: "Finance",
                table: "BudgetCode",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCode_IsActive",
                schema: "Finance",
                table: "BudgetCode",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCode_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetCode",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCode_Name",
                schema: "Finance",
                table: "BudgetCode",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetGroup_Code",
                schema: "Finance",
                table: "BudgetGroup",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetGroup_CreatedByUserId",
                schema: "Finance",
                table: "BudgetGroup",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetGroup_DeletedByUserId",
                schema: "Finance",
                table: "BudgetGroup",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetGroup_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetGroup",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetGroup_Name",
                schema: "Finance",
                table: "BudgetGroup",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPeriod_CreatedByUserId",
                schema: "Finance",
                table: "BudgetPeriod",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPeriod_DeletedByUserId",
                schema: "Finance",
                table: "BudgetPeriod",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPeriod_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetPeriod",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPeriod_Year_Month",
                schema: "Finance",
                table: "BudgetPeriod",
                columns: new[] { "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlan_ApprovedByUserId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlan_BudgetApproverId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "BudgetApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlan_BudgetCodeId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "BudgetCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlan_BudgetPeriodId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "BudgetPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlan_CreatedByUserId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlan_DeletedByUserId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlan_DepartmentId_BudgetPeriodId",
                schema: "Finance",
                table: "BudgetPlan",
                columns: new[] { "DepartmentId", "BudgetPeriodId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlan_IsActive",
                schema: "Finance",
                table: "BudgetPlan",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlan_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlan_ReviewedByUserId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlan_Status",
                schema: "Finance",
                table: "BudgetPlan",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlanDetails_BudgetCodeId",
                schema: "Finance",
                table: "BudgetPlanDetails",
                column: "BudgetCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlanDetails_BudgetPlanId",
                schema: "Finance",
                table: "BudgetPlanDetails",
                column: "BudgetPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlanDetails_BudgetPlanId_BudgetCodeId_IsActive",
                schema: "Finance",
                table: "BudgetPlanDetails",
                columns: new[] { "BudgetPlanId", "BudgetCodeId", "IsActive" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlanDetails_DeletedByUserId",
                schema: "Finance",
                table: "BudgetPlanDetails",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlanDetails_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetPlanDetails",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTransaction_BudgetPlanId",
                schema: "Finance",
                table: "BudgetTransaction",
                column: "BudgetPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTransaction_TransactionDate",
                schema: "Finance",
                table: "BudgetTransaction",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTransaction_Type",
                schema: "Finance",
                table: "BudgetTransaction",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutCodes_CashoutGroupId_Code",
                schema: "Finance",
                table: "CashoutCodes",
                columns: new[] { "CashoutGroupId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashoutCodes_CreatedByUserId",
                schema: "Finance",
                table: "CashoutCodes",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutCodes_DeletedByUserId",
                schema: "Finance",
                table: "CashoutCodes",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutCodes_Id",
                schema: "Finance",
                table: "CashoutCodes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutCodes_IsActive",
                schema: "Finance",
                table: "CashoutCodes",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutCodes_LedgerAccountId",
                schema: "Finance",
                table: "CashoutCodes",
                column: "LedgerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutCodes_ModifiedByUserId",
                schema: "Finance",
                table: "CashoutCodes",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutCodes_Name",
                schema: "Finance",
                table: "CashoutCodes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutCodes_PostingLedgerAccountId",
                schema: "Finance",
                table: "CashoutCodes",
                column: "PostingLedgerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutGroups_Code",
                schema: "Finance",
                table: "CashoutGroups",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashoutGroups_CreatedByUserId",
                schema: "Finance",
                table: "CashoutGroups",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutGroups_DeletedByUserId",
                schema: "Finance",
                table: "CashoutGroups",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutGroups_IsActive",
                schema: "Finance",
                table: "CashoutGroups",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutGroups_ModifiedByUserId",
                schema: "Finance",
                table: "CashoutGroups",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutGroups_Name",
                schema: "Finance",
                table: "CashoutGroups",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CashoutGroups_ParentId",
                schema: "Finance",
                table: "CashoutGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentManagers_DepartmentId_IsPrimary",
                schema: "Account",
                table: "DepartmentManagers",
                columns: new[] { "DepartmentId", "IsPrimary" },
                unique: true,
                filter: "[IsPrimary] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentManagers_UserId",
                schema: "Account",
                table: "DepartmentManagers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code",
                schema: "Account",
                table: "Departments",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CreatedByUserId",
                schema: "Account",
                table: "Departments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DeletedByUserId",
                schema: "Account",
                table: "Departments",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ModifiedByUserId",
                schema: "Account",
                table: "Departments",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentId",
                schema: "Account",
                table: "Departments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSequences_CreatedByUserId",
                table: "DocumentSequences",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSequences_DeletedByUserId",
                table: "DocumentSequences",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSequences_ModifiedByUserId",
                table: "DocumentSequences",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentAttachments_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentAttachments_DeletedByUserId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentAttachments_ExpensePaymentId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "ExpensePaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentAttachments_FileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentAttachments_ModifiedByUserId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentCommentAttachments_CommentId",
                schema: "Expense",
                table: "ExpensePaymentCommentAttachments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentCommentAttachments_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePaymentCommentAttachments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentCommentAttachments_DeletedByUserId",
                schema: "Expense",
                table: "ExpensePaymentCommentAttachments",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentCommentAttachments_ModifiedByUserId",
                schema: "Expense",
                table: "ExpensePaymentCommentAttachments",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentComments_CreatedAt",
                schema: "Expense",
                table: "ExpensePaymentComments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentComments_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePaymentComments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentComments_DeletedByUserId",
                schema: "Expense",
                table: "ExpensePaymentComments",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentComments_ExpensePaymentId",
                schema: "Expense",
                table: "ExpensePaymentComments",
                column: "ExpensePaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentComments_ModifiedByUserId",
                schema: "Expense",
                table: "ExpensePaymentComments",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentComments_ParentCommentId",
                schema: "Expense",
                table: "ExpensePaymentComments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentCommentTags_CommentId",
                schema: "Expense",
                table: "ExpensePaymentCommentTags",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentCommentTags_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePaymentCommentTags",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentCommentTags_DeletedByUserId",
                schema: "Expense",
                table: "ExpensePaymentCommentTags",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentCommentTags_ModifiedByUserId",
                schema: "Expense",
                table: "ExpensePaymentCommentTags",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentCommentTags_UserId",
                schema: "Expense",
                table: "ExpensePaymentCommentTags",
                column: "UserId");

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
                name: "IX_ExpensePaymentItems_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentItems_DeletedByUserId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentItems_ExpensePaymentId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "ExpensePaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentItems_InvoiceId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentItems_ItemName",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "ItemName");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePaymentItems_ModifiedByUserId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePayments_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePayments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePayments_CurrentWorkflowInstanceId",
                schema: "Expense",
                table: "ExpensePayments",
                column: "CurrentWorkflowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePayments_DeletedByUserId",
                schema: "Expense",
                table: "ExpensePayments",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePayments_DueDate",
                schema: "Expense",
                table: "ExpensePayments",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePayments_ManagerApproverId",
                schema: "Expense",
                table: "ExpensePayments",
                column: "ManagerApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePayments_ModifiedByUserId",
                schema: "Expense",
                table: "ExpensePayments",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePayments_Status",
                schema: "Expense",
                table: "ExpensePayments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePayments_SubId",
                schema: "Expense",
                table: "ExpensePayments",
                column: "SubId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePayments_SupplierId",
                schema: "Expense",
                table: "ExpensePayments",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepInstances_ApprovedBy",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepInstances_CreatedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepInstances_DeletedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepInstances_ModifiedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepInstances_RejectedBy",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "RejectedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepInstances_TemplateStepId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "TemplateStepId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepInstances_WorkflowInstanceId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "WorkflowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepTemplates_CreatedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepTemplates_DeletedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepTemplates_ModifiedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStepTemplates_WorkflowTemplateId_Order",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                columns: new[] { "WorkflowTemplateId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseWorkflowTemplates_CreatedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseWorkflowTemplates",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseWorkflowTemplates_DeletedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseWorkflowTemplates",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseWorkflowTemplates_DocumentType_Version",
                schema: "ExpenseWorkflow",
                table: "ExpenseWorkflowTemplates",
                columns: new[] { "DocumentType", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseWorkflowTemplates_ModifiedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseWorkflowTemplates",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpesneWorkflowInstance_DocumentType_DocumentId",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance",
                columns: new[] { "DocumentType", "DocumentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Followers_CreatedByUserId",
                schema: "Core",
                table: "Followers",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_DeletedByUserId",
                schema: "Core",
                table: "Followers",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_ModifiedByUserId",
                schema: "Core",
                table: "Followers",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_AdminId",
                schema: "Account",
                table: "Groups",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CreatedByUserId",
                schema: "Account",
                table: "Groups",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_DeletedByUserId",
                schema: "Account",
                table: "Groups",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ModifiedByUserId",
                schema: "Account",
                table: "Groups",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Slug",
                schema: "Account",
                table: "Groups",
                column: "Slug",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_LedgerAccounts_CreatedByUserId",
                schema: "Finance",
                table: "LedgerAccounts",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerAccounts_DeletedByUserId",
                schema: "Finance",
                table: "LedgerAccounts",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerAccounts_IsActive",
                schema: "Finance",
                table: "LedgerAccounts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerAccounts_LedgerAccountTypeId",
                schema: "Finance",
                table: "LedgerAccounts",
                column: "LedgerAccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerAccounts_ModifiedByUserId",
                schema: "Finance",
                table: "LedgerAccounts",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerAccounts_Number",
                schema: "Finance",
                table: "LedgerAccounts",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LedgerAccounts_ParentLedgerAccountId",
                schema: "Finance",
                table: "LedgerAccounts",
                column: "ParentLedgerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerAccountTypes_Code",
                schema: "Finance",
                table: "LedgerAccountTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LedgerAccountTypes_CreatedByUserId",
                schema: "Finance",
                table: "LedgerAccountTypes",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerAccountTypes_DeletedByUserId",
                schema: "Finance",
                table: "LedgerAccountTypes",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerAccountTypes_IsActive",
                schema: "Finance",
                table: "LedgerAccountTypes",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerAccountTypes_ModifiedByUserId",
                schema: "Finance",
                table: "LedgerAccountTypes",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NumberSeries_Key",
                table: "NumberSeries",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingBankAccounts_AccountNumber",
                schema: "Expense",
                table: "OutgoingBankAccounts",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingBankAccounts_CreatedByUserId",
                schema: "Expense",
                table: "OutgoingBankAccounts",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingBankAccounts_DeletedByUserId",
                schema: "Expense",
                table: "OutgoingBankAccounts",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingBankAccounts_IsActive_Name",
                schema: "Expense",
                table: "OutgoingBankAccounts",
                columns: new[] { "IsActive", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingBankAccounts_ModifiedByUserId",
                schema: "Expense",
                table: "OutgoingBankAccounts",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingPayments_CreatedByUserId",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingPayments_DeletedByUserId",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingPayments_DueDate",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingPayments_EmployeeId",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingPayments_ExpensePaymentId",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "ExpensePaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingPayments_ModifiedByUserId",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingPayments_OutgoingBankAccountId",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "OutgoingBankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingPayments_PaymentDate",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingPayments_PostingDate",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "PostingDate");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingPayments_SupplierId",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Code",
                schema: "RBAC",
                table: "Permissions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_CreatedByUserId",
                schema: "RBAC",
                table: "Permissions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_DeletedByUserId",
                schema: "RBAC",
                table: "Permissions",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ModifiedByUserId",
                schema: "RBAC",
                table: "Permissions",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TokenHash",
                schema: "Auth",
                table: "RefreshTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId_IsRevoked_ExpiresAt",
                schema: "Auth",
                table: "RefreshTokens",
                columns: new[] { "UserId", "IsRevoked", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                schema: "RBAC",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                schema: "RBAC",
                table: "Roles",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_StoredFiles_Bucket_ObjectKey",
                schema: "Files",
                table: "StoredFiles",
                columns: new[] { "Bucket", "ObjectKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoredFiles_CreatedByUserId",
                schema: "Files",
                table: "StoredFiles",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoredFiles_DeletedByUserId",
                schema: "Files",
                table: "StoredFiles",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoredFiles_IsPublic",
                schema: "Files",
                table: "StoredFiles",
                column: "IsPublic");

            migrationBuilder.CreateIndex(
                name: "IX_StoredFiles_ModifiedByUserId",
                schema: "Files",
                table: "StoredFiles",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoredFiles_Module_Entity_EntityId",
                schema: "Files",
                table: "StoredFiles",
                columns: new[] { "Module", "Entity", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CreatedByUserId",
                schema: "Expense",
                table: "Suppliers",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_DeletedByUserId",
                schema: "Expense",
                table: "Suppliers",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_ModifiedByUserId",
                schema: "Expense",
                table: "Suppliers",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Name",
                schema: "Expense",
                table: "Suppliers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_TaxCode",
                schema: "Expense",
                table: "Suppliers",
                column: "TaxCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_CreatedByUserId",
                schema: "Account",
                table: "UserGroups",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_DeletedByUserId",
                schema: "Account",
                table: "UserGroups",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_GroupId",
                schema: "Account",
                table: "UserGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_ModifiedByUserId",
                schema: "Account",
                table: "UserGroups",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_UserId_GroupId",
                schema: "Account",
                table: "UserGroups",
                columns: new[] { "UserId", "GroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserManagerAssignments_CreatedByUserId",
                schema: "Account",
                table: "UserManagerAssignments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserManagerAssignments_DeletedByUserId",
                schema: "Account",
                table: "UserManagerAssignments",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserManagerAssignments_ManagerId",
                schema: "Account",
                table: "UserManagerAssignments",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserManagerAssignments_ModifiedByUserId",
                schema: "Account",
                table: "UserManagerAssignments",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserManagerAssignments_UserId",
                schema: "Account",
                table: "UserManagerAssignments",
                column: "UserId",
                unique: true,
                filter: "[IsActive] = 1 AND [IsPrimary] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_UserManagerAssignments_UserId_ManagerId",
                schema: "Account",
                table: "UserManagerAssignments",
                columns: new[] { "UserId", "ManagerId" },
                unique: true,
                filter: "[IsActive] = 1 AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_ReceiverId",
                schema: "Core",
                table: "UserNotifications",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_SenderId",
                schema: "Core",
                table: "UserNotifications",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReminders_UserId",
                schema: "Core",
                table: "UserReminders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "RBAC",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AvatarFileId",
                schema: "Account",
                table: "Users",
                column: "AvatarFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                schema: "Account",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeCode",
                schema: "Account",
                table: "Users",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ManagerId",
                schema: "Account",
                table: "Users",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OutgoingPaymentId",
                schema: "Account",
                table: "Users",
                column: "OutgoingPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                schema: "Account",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetApprover_Users_ApproverUserId",
                schema: "Finance",
                table: "BudgetApprover",
                column: "ApproverUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetApprover_Users_CreatedByUserId",
                schema: "Finance",
                table: "BudgetApprover",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetApprover_Users_DeletedByUserId",
                schema: "Finance",
                table: "BudgetApprover",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetApprover_Users_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetApprover",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetApproverDepartments_Departments_DepartmentId",
                schema: "Finance",
                table: "BudgetApproverDepartments",
                column: "DepartmentId",
                principalSchema: "Account",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetCode_BudgetGroup_BudgetGroupId",
                schema: "Finance",
                table: "BudgetCode",
                column: "BudgetGroupId",
                principalSchema: "Finance",
                principalTable: "BudgetGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetCode_CashoutCodes_CashoutCodeId",
                schema: "Finance",
                table: "BudgetCode",
                column: "CashoutCodeId",
                principalSchema: "Finance",
                principalTable: "CashoutCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetCode_Users_CreatedByUserId",
                schema: "Finance",
                table: "BudgetCode",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetCode_Users_DeletedByUserId",
                schema: "Finance",
                table: "BudgetCode",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetCode_Users_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetCode",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetGroup_Users_CreatedByUserId",
                schema: "Finance",
                table: "BudgetGroup",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetGroup_Users_DeletedByUserId",
                schema: "Finance",
                table: "BudgetGroup",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetGroup_Users_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetGroup",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPeriod_Users_CreatedByUserId",
                schema: "Finance",
                table: "BudgetPeriod",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPeriod_Users_DeletedByUserId",
                schema: "Finance",
                table: "BudgetPeriod",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPeriod_Users_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetPeriod",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlan_Departments_DepartmentId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "DepartmentId",
                principalSchema: "Account",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlan_Users_ApprovedByUserId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "ApprovedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlan_Users_CreatedByUserId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlan_Users_DeletedByUserId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlan_Users_ModifiedByUserId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlan_Users_ReviewedByUserId",
                schema: "Finance",
                table: "BudgetPlan",
                column: "ReviewedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_CashoutCodes_CashoutGroups_CashoutGroupId",
                schema: "Finance",
                table: "CashoutCodes",
                column: "CashoutGroupId",
                principalSchema: "Finance",
                principalTable: "CashoutGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CashoutCodes_LedgerAccounts_LedgerAccountId",
                schema: "Finance",
                table: "CashoutCodes",
                column: "LedgerAccountId",
                principalSchema: "Finance",
                principalTable: "LedgerAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashoutCodes_LedgerAccounts_PostingLedgerAccountId",
                schema: "Finance",
                table: "CashoutCodes",
                column: "PostingLedgerAccountId",
                principalSchema: "Finance",
                principalTable: "LedgerAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CashoutCodes_Users_CreatedByUserId",
                schema: "Finance",
                table: "CashoutCodes",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CashoutCodes_Users_DeletedByUserId",
                schema: "Finance",
                table: "CashoutCodes",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashoutCodes_Users_ModifiedByUserId",
                schema: "Finance",
                table: "CashoutCodes",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashoutGroups_Users_CreatedByUserId",
                schema: "Finance",
                table: "CashoutGroups",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CashoutGroups_Users_DeletedByUserId",
                schema: "Finance",
                table: "CashoutGroups",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashoutGroups_Users_ModifiedByUserId",
                schema: "Finance",
                table: "CashoutGroups",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentManagers_Departments_DepartmentId",
                schema: "Account",
                table: "DepartmentManagers",
                column: "DepartmentId",
                principalSchema: "Account",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentManagers_Users_UserId",
                schema: "Account",
                table: "DepartmentManagers",
                column: "UserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_CreatedByUserId",
                schema: "Account",
                table: "Departments",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_DeletedByUserId",
                schema: "Account",
                table: "Departments",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_ModifiedByUserId",
                schema: "Account",
                table: "Departments",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentSequences_Users_CreatedByUserId",
                table: "DocumentSequences",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentSequences_Users_DeletedByUserId",
                table: "DocumentSequences",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentSequences_Users_ModifiedByUserId",
                table: "DocumentSequences",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentAttachments_ExpensePayments_ExpensePaymentId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "ExpensePaymentId",
                principalSchema: "Expense",
                principalTable: "ExpensePayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentAttachments_Users_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentAttachments_Users_DeletedByUserId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentAttachments_Users_ModifiedByUserId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentCommentAttachments_ExpensePaymentComments_CommentId",
                schema: "Expense",
                table: "ExpensePaymentCommentAttachments",
                column: "CommentId",
                principalSchema: "Expense",
                principalTable: "ExpensePaymentComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentCommentAttachments_Users_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePaymentCommentAttachments",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentCommentAttachments_Users_DeletedByUserId",
                schema: "Expense",
                table: "ExpensePaymentCommentAttachments",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentCommentAttachments_Users_ModifiedByUserId",
                schema: "Expense",
                table: "ExpensePaymentCommentAttachments",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentComments_ExpensePayments_ExpensePaymentId",
                schema: "Expense",
                table: "ExpensePaymentComments",
                column: "ExpensePaymentId",
                principalSchema: "Expense",
                principalTable: "ExpensePayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentComments_Users_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePaymentComments",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentComments_Users_DeletedByUserId",
                schema: "Expense",
                table: "ExpensePaymentComments",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentComments_Users_ModifiedByUserId",
                schema: "Expense",
                table: "ExpensePaymentComments",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentCommentTags_Users_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePaymentCommentTags",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentCommentTags_Users_DeletedByUserId",
                schema: "Expense",
                table: "ExpensePaymentCommentTags",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentCommentTags_Users_ModifiedByUserId",
                schema: "Expense",
                table: "ExpensePaymentCommentTags",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentCommentTags_Users_UserId",
                schema: "Expense",
                table: "ExpensePaymentCommentTags",
                column: "UserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentItems_ExpensePayments_ExpensePaymentId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "ExpensePaymentId",
                principalSchema: "Expense",
                principalTable: "ExpensePayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentItems_Invoices_InvoiceId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "InvoiceId",
                principalSchema: "Expense",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentItems_Users_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentItems_Users_DeletedByUserId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentItems_Users_ModifiedByUserId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePayments_Suppliers_SupplierId",
                schema: "Expense",
                table: "ExpensePayments",
                column: "SupplierId",
                principalSchema: "Expense",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePayments_Users_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePayments",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePayments_Users_DeletedByUserId",
                schema: "Expense",
                table: "ExpensePayments",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePayments_Users_ManagerApproverId",
                schema: "Expense",
                table: "ExpensePayments",
                column: "ManagerApproverId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePayments_Users_ModifiedByUserId",
                schema: "Expense",
                table: "ExpensePayments",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepInstances_ExpenseStepTemplates_TemplateStepId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "TemplateStepId",
                principalSchema: "ExpenseWorkflow",
                principalTable: "ExpenseStepTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepInstances_Users_ApprovedBy",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "ApprovedBy",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepInstances_Users_CreatedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepInstances_Users_DeletedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepInstances_Users_ModifiedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepInstances_Users_RejectedBy",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepInstances",
                column: "RejectedBy",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepTemplates_ExpenseWorkflowTemplates_WorkflowTemplateId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                column: "WorkflowTemplateId",
                principalSchema: "ExpenseWorkflow",
                principalTable: "ExpenseWorkflowTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepTemplates_Users_CreatedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepTemplates_Users_DeletedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseStepTemplates_Users_ModifiedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseStepTemplates",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseWorkflowTemplates_Users_CreatedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseWorkflowTemplates",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseWorkflowTemplates_Users_DeletedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseWorkflowTemplates",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseWorkflowTemplates_Users_ModifiedByUserId",
                schema: "ExpenseWorkflow",
                table: "ExpenseWorkflowTemplates",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_CreatedByUserId",
                schema: "Core",
                table: "Followers",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_DeletedByUserId",
                schema: "Core",
                table: "Followers",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_ModifiedByUserId",
                schema: "Core",
                table: "Followers",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_AdminId",
                schema: "Account",
                table: "Groups",
                column: "AdminId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_CreatedByUserId",
                schema: "Account",
                table: "Groups",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_DeletedByUserId",
                schema: "Account",
                table: "Groups",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_ModifiedByUserId",
                schema: "Account",
                table: "Groups",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceFiles_Invoices_InvoiceId",
                schema: "Expense",
                table: "InvoiceFiles",
                column: "InvoiceId",
                principalSchema: "Expense",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceFiles_StoredFiles_FileId",
                schema: "Expense",
                table: "InvoiceFiles",
                column: "FileId",
                principalSchema: "Files",
                principalTable: "StoredFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceFiles_Users_CreatedByUserId",
                schema: "Expense",
                table: "InvoiceFiles",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceFiles_Users_DeletedByUserId",
                schema: "Expense",
                table: "InvoiceFiles",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceFiles_Users_ModifiedByUserId",
                schema: "Expense",
                table: "InvoiceFiles",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Users_CreatedByUserId",
                schema: "Expense",
                table: "Invoices",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Users_DeletedByUserId",
                schema: "Expense",
                table: "Invoices",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Users_ModifiedByUserId",
                schema: "Expense",
                table: "Invoices",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerAccounts_LedgerAccountTypes_LedgerAccountTypeId",
                schema: "Finance",
                table: "LedgerAccounts",
                column: "LedgerAccountTypeId",
                principalSchema: "Finance",
                principalTable: "LedgerAccountTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerAccounts_Users_CreatedByUserId",
                schema: "Finance",
                table: "LedgerAccounts",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerAccounts_Users_DeletedByUserId",
                schema: "Finance",
                table: "LedgerAccounts",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerAccounts_Users_ModifiedByUserId",
                schema: "Finance",
                table: "LedgerAccounts",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerAccountTypes_Users_CreatedByUserId",
                schema: "Finance",
                table: "LedgerAccountTypes",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerAccountTypes_Users_DeletedByUserId",
                schema: "Finance",
                table: "LedgerAccountTypes",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerAccountTypes_Users_ModifiedByUserId",
                schema: "Finance",
                table: "LedgerAccountTypes",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingBankAccounts_Users_CreatedByUserId",
                schema: "Expense",
                table: "OutgoingBankAccounts",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingBankAccounts_Users_DeletedByUserId",
                schema: "Expense",
                table: "OutgoingBankAccounts",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingBankAccounts_Users_ModifiedByUserId",
                schema: "Expense",
                table: "OutgoingBankAccounts",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingPayments_Suppliers_SupplierId",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "SupplierId",
                principalSchema: "Expense",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingPayments_Users_CreatedByUserId",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingPayments_Users_DeletedByUserId",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingPayments_Users_EmployeeId",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "EmployeeId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingPayments_Users_ModifiedByUserId",
                schema: "Expense",
                table: "OutgoingPayments",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Users_CreatedByUserId",
                schema: "RBAC",
                table: "Permissions",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Users_DeletedByUserId",
                schema: "RBAC",
                table: "Permissions",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Users_ModifiedByUserId",
                schema: "RBAC",
                table: "Permissions",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoredFiles_Users_CreatedByUserId",
                schema: "Files",
                table: "StoredFiles",
                column: "CreatedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StoredFiles_Users_DeletedByUserId",
                schema: "Files",
                table: "StoredFiles",
                column: "DeletedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoredFiles_Users_ModifiedByUserId",
                schema: "Files",
                table: "StoredFiles",
                column: "ModifiedByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_CreatedByUserId",
                schema: "Account",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_DeletedByUserId",
                schema: "Account",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_ModifiedByUserId",
                schema: "Account",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePayments_Users_CreatedByUserId",
                schema: "Expense",
                table: "ExpensePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePayments_Users_DeletedByUserId",
                schema: "Expense",
                table: "ExpensePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePayments_Users_ManagerApproverId",
                schema: "Expense",
                table: "ExpensePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePayments_Users_ModifiedByUserId",
                schema: "Expense",
                table: "ExpensePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingBankAccounts_Users_CreatedByUserId",
                schema: "Expense",
                table: "OutgoingBankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingBankAccounts_Users_DeletedByUserId",
                schema: "Expense",
                table: "OutgoingBankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingBankAccounts_Users_ModifiedByUserId",
                schema: "Expense",
                table: "OutgoingBankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingPayments_Users_CreatedByUserId",
                schema: "Expense",
                table: "OutgoingPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingPayments_Users_DeletedByUserId",
                schema: "Expense",
                table: "OutgoingPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingPayments_Users_EmployeeId",
                schema: "Expense",
                table: "OutgoingPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingPayments_Users_ModifiedByUserId",
                schema: "Expense",
                table: "OutgoingPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_StoredFiles_Users_CreatedByUserId",
                schema: "Files",
                table: "StoredFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_StoredFiles_Users_DeletedByUserId",
                schema: "Files",
                table: "StoredFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_StoredFiles_Users_ModifiedByUserId",
                schema: "Files",
                table: "StoredFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Users_CreatedByUserId",
                schema: "Expense",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Users_DeletedByUserId",
                schema: "Expense",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Users_ModifiedByUserId",
                schema: "Expense",
                table: "Suppliers");

            migrationBuilder.DropTable(
                name: "BudgetApproverDepartments",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "BudgetPlanDetails",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "BudgetTransaction",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "DepartmentManagers",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "DocumentSequences");

            migrationBuilder.DropTable(
                name: "ExpensePaymentAttachments",
                schema: "Expense");

            migrationBuilder.DropTable(
                name: "ExpensePaymentCommentAttachments",
                schema: "Expense");

            migrationBuilder.DropTable(
                name: "ExpensePaymentCommentTags",
                schema: "Expense");

            migrationBuilder.DropTable(
                name: "ExpensePaymentItems",
                schema: "Expense");

            migrationBuilder.DropTable(
                name: "ExpenseStepInstances",
                schema: "ExpenseWorkflow");

            migrationBuilder.DropTable(
                name: "Followers",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "InvoiceFiles",
                schema: "Expense");

            migrationBuilder.DropTable(
                name: "NumberSeries");

            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "RolePermissions",
                schema: "RBAC");

            migrationBuilder.DropTable(
                name: "UserGroups",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "UserManagerAssignments",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "UserNotifications",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "UserReminders",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "RBAC");

            migrationBuilder.DropTable(
                name: "BudgetPlan",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "ExpensePaymentComments",
                schema: "Expense");

            migrationBuilder.DropTable(
                name: "ExpenseStepTemplates",
                schema: "ExpenseWorkflow");

            migrationBuilder.DropTable(
                name: "Invoices",
                schema: "Expense");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "RBAC");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "RBAC");

            migrationBuilder.DropTable(
                name: "BudgetApprover",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "BudgetCode",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "BudgetPeriod",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "ExpenseWorkflowTemplates",
                schema: "ExpenseWorkflow");

            migrationBuilder.DropTable(
                name: "BudgetGroup",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "CashoutCodes",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "CashoutGroups",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "LedgerAccounts",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "LedgerAccountTypes",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "OutgoingPayments",
                schema: "Expense");

            migrationBuilder.DropTable(
                name: "StoredFiles",
                schema: "Files");

            migrationBuilder.DropTable(
                name: "ExpensePayments",
                schema: "Expense");

            migrationBuilder.DropTable(
                name: "OutgoingBankAccounts",
                schema: "Expense");

            migrationBuilder.DropTable(
                name: "ExpesneWorkflowInstance",
                schema: "ExpenseWorkflow");

            migrationBuilder.DropTable(
                name: "Suppliers",
                schema: "Expense");
        }
    }
}
