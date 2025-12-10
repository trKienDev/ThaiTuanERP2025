using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEntityExpensePaymentComment_Tag_Attachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpensePaymentCommentAttachments",
                schema: "Expense");

            migrationBuilder.DropTable(
                name: "ExpensePaymentCommentTags",
                schema: "Expense");

            migrationBuilder.DropTable(
                name: "ExpensePaymentComments",
                schema: "Expense");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpensePaymentComments",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExpensePaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommentType = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_ExpensePaymentComments_ExpensePayments_ExpensePaymentId",
                        column: x => x.ExpensePaymentId,
                        principalSchema: "Expense",
                        principalTable: "ExpensePayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpensePaymentComments_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ExpensePaymentComments_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExpensePaymentComments_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExpensePaymentCommentAttachments",
                schema: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensePaymentCommentAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpensePaymentCommentAttachments_ExpensePaymentComments_CommentId",
                        column: x => x.CommentId,
                        principalSchema: "Expense",
                        principalTable: "ExpensePaymentComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpensePaymentCommentAttachments_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpensePaymentCommentAttachments_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpensePaymentCommentAttachments_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
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
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_ExpensePaymentCommentTags_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpensePaymentCommentTags_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpensePaymentCommentTags_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpensePaymentCommentTags_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
        }
    }
}
