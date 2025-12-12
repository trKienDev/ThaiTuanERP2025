using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameFileAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentAttachments_StoredFiles_StoredFileId",
                schema: "Core",
                table: "CommentAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePaymentAttachments_StoredFiles_StoredFileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePaymentItems_StoredFiles_InvoiceFileId",
                schema: "Expense",
                table: "ExpensePaymentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_StoredFiles_AvatarFileId",
                schema: "Account",
                table: "Users");

            migrationBuilder.DropTable(
                name: "StoredFiles",
                schema: "Files");

            migrationBuilder.CreateTable(
                name: "FileAttachment",
                schema: "Core",
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
                    table.PrimaryKey("PK_FileAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileAttachment_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_FileAttachment_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileAttachment_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachment_Bucket_ObjectKey",
                schema: "Core",
                table: "FileAttachment",
                columns: new[] { "Bucket", "ObjectKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachment_CreatedByUserId",
                schema: "Core",
                table: "FileAttachment",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachment_DeletedByUserId",
                schema: "Core",
                table: "FileAttachment",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachment_ModifiedByUserId",
                schema: "Core",
                table: "FileAttachment",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachment_Module_Entity_EntityId",
                schema: "Core",
                table: "FileAttachment",
                columns: new[] { "Module", "Entity", "EntityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentAttachments_FileAttachment_StoredFileId",
                schema: "Core",
                table: "CommentAttachments",
                column: "StoredFileId",
                principalSchema: "Core",
                principalTable: "FileAttachment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentAttachments_FileAttachment_StoredFileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "StoredFileId",
                principalSchema: "Core",
                principalTable: "FileAttachment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentItems_FileAttachment_InvoiceFileId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "InvoiceFileId",
                principalSchema: "Core",
                principalTable: "FileAttachment",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_FileAttachment_AvatarFileId",
                schema: "Account",
                table: "Users",
                column: "AvatarFileId",
                principalSchema: "Core",
                principalTable: "FileAttachment",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentAttachments_FileAttachment_StoredFileId",
                schema: "Core",
                table: "CommentAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePaymentAttachments_FileAttachment_StoredFileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePaymentItems_FileAttachment_InvoiceFileId",
                schema: "Expense",
                table: "ExpensePaymentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_FileAttachment_AvatarFileId",
                schema: "Account",
                table: "Users");

            migrationBuilder.DropTable(
                name: "FileAttachment",
                schema: "Core");

            migrationBuilder.EnsureSchema(
                name: "Files");

            migrationBuilder.CreateTable(
                name: "StoredFiles",
                schema: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Bucket = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Entity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Module = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ObjectKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoredFiles_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StoredFiles_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoredFiles_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

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
                name: "IX_StoredFiles_ModifiedByUserId",
                schema: "Files",
                table: "StoredFiles",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoredFiles_Module_Entity_EntityId",
                schema: "Files",
                table: "StoredFiles",
                columns: new[] { "Module", "Entity", "EntityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentAttachments_StoredFiles_StoredFileId",
                schema: "Core",
                table: "CommentAttachments",
                column: "StoredFileId",
                principalSchema: "Files",
                principalTable: "StoredFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentAttachments_StoredFiles_StoredFileId",
                schema: "Expense",
                table: "ExpensePaymentAttachments",
                column: "StoredFileId",
                principalSchema: "Files",
                principalTable: "StoredFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePaymentItems_StoredFiles_InvoiceFileId",
                schema: "Expense",
                table: "ExpensePaymentItems",
                column: "InvoiceFileId",
                principalSchema: "Files",
                principalTable: "StoredFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_StoredFiles_AvatarFileId",
                schema: "Account",
                table: "Users",
                column: "AvatarFileId",
                principalSchema: "Files",
                principalTable: "StoredFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
