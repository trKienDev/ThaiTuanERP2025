using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drive.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBucketNameFromStoredObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StoredObject_Bucket_ObjectKey",
                table: "StoredObject");

            migrationBuilder.DropColumn(
                name: "Bucket",
                table: "StoredObject");

            migrationBuilder.CreateIndex(
                name: "IX_StoredObject_ObjectKey",
                table: "StoredObject",
                column: "ObjectKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StoredObject_ObjectKey",
                table: "StoredObject");

            migrationBuilder.AddColumn<string>(
                name: "Bucket",
                table: "StoredObject",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StoredObject_Bucket_ObjectKey",
                table: "StoredObject",
                columns: new[] { "Bucket", "ObjectKey" },
                unique: true);
        }
    }
}
