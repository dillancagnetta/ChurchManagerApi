using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class settngs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Common",
                table: "Setting");

            migrationBuilder.AddColumn<string>(
                name: "TenantName",
                schema: "Common",
                table: "Setting",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantName",
                schema: "Common",
                table: "Setting");

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                schema: "Common",
                table: "Setting",
                type: "integer",
                nullable: true);
        }
    }
}
