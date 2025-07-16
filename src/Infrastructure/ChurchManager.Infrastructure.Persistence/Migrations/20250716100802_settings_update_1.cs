using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class settings_update_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                schema: "Common",
                table: "Setting",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                schema: "Finances",
                table: "Giving",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "GivingType",
                schema: "Finances",
                table: "Giving",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Common",
                table: "Setting");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                schema: "Finances",
                table: "Giving",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "GivingType",
                schema: "Finances",
                table: "Giving",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
