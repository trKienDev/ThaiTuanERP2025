using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEntity_NotificationAndTaskReminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppNotifications",
                schema: "Alerts");

            migrationBuilder.DropTable(
                name: "TaskReminder",
                schema: "Alerts");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueAt",
                schema: "Finance",
                table: "BudgetPlan",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueAt",
                schema: "Finance",
                table: "BudgetPlan");

            migrationBuilder.EnsureSchema(
                name: "Alerts");

            migrationBuilder.CreateTable(
                name: "AppNotifications",
                schema: "Alerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkflowInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkflowStepInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppNotifications_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppNotifications_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppNotifications_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskReminder",
                schema: "Alerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StepInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkflowInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskReminder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskReminder_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskReminder_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskReminder_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppNotifications_CreatedByUserId",
                schema: "Alerts",
                table: "AppNotifications",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppNotifications_DeletedByUserId",
                schema: "Alerts",
                table: "AppNotifications",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppNotifications_ModifiedByUserId",
                schema: "Alerts",
                table: "AppNotifications",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppNotifications_UserId_DocumentType_DocumentId_WorkflowStepInstanceId",
                schema: "Alerts",
                table: "AppNotifications",
                columns: new[] { "UserId", "DocumentType", "DocumentId", "WorkflowStepInstanceId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppNotifications_UserId_IsRead_CreatedDate",
                schema: "Alerts",
                table: "AppNotifications",
                columns: new[] { "UserId", "IsRead", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskReminder_CreatedByUserId",
                schema: "Alerts",
                table: "TaskReminder",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskReminder_DeletedByUserId",
                schema: "Alerts",
                table: "TaskReminder",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskReminder_ModifiedByUserId",
                schema: "Alerts",
                table: "TaskReminder",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskReminder_StepInstanceId_UserId",
                schema: "Alerts",
                table: "TaskReminder",
                columns: new[] { "StepInstanceId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskReminder_UserId_ResolvedAt_DueAt",
                schema: "Alerts",
                table: "TaskReminder",
                columns: new[] { "UserId", "ResolvedAt", "DueAt" });
        }
    }
}
