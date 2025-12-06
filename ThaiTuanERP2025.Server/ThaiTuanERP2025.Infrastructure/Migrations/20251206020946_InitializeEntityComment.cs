using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitializeEntityComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Module = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Entity = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Comments_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatedByUserId",
                schema: "Core",
                table: "Comments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DeletedByUserId",
                schema: "Core",
                table: "Comments",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ModifiedByUserId",
                schema: "Core",
                table: "Comments",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Module_Entity_EntityId",
                schema: "Core",
                table: "Comments",
                columns: new[] { "Module", "Entity", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                schema: "Core",
                table: "Comments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments",
                schema: "Core");
        }
    }
}
