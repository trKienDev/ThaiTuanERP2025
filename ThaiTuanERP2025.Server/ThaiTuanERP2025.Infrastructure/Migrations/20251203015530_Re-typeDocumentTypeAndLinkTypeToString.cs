using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RetypeDocumentTypeAndLinkTypeToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LinkType",
                schema: "Core",
                table: "UserNotifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentType",
                schema: "Core",
                table: "Followers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentType",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LinkType",
                schema: "Core",
                table: "UserNotifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentType",
                schema: "Core",
                table: "Followers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentType",
                schema: "ExpenseWorkflow",
                table: "ExpesneWorkflowInstance",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
