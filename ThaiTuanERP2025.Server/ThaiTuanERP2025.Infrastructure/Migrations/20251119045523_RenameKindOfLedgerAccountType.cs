using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiTuanERP2025.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameKindOfLedgerAccountType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LedgerAccountTypeKind",
                schema: "Finance",
                table: "LedgerAccountTypes",
                newName: "Kind");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Kind",
                schema: "Finance",
                table: "LedgerAccountTypes",
                newName: "LedgerAccountTypeKind");
        }
    }
}
