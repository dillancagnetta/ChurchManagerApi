using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class group_type_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowAnyChildGroupType",
                table: "GroupType",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int[]>(
                name: "AllowedChildGroupTypesIds",
                table: "GroupType",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "GroupType",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SendAttendanceReminder",
                table: "GroupType",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowInNavigation",
                table: "GroupType",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowAnyChildGroupType",
                table: "GroupType");

            migrationBuilder.DropColumn(
                name: "AllowedChildGroupTypesIds",
                table: "GroupType");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "GroupType");

            migrationBuilder.DropColumn(
                name: "SendAttendanceReminder",
                table: "GroupType");

            migrationBuilder.DropColumn(
                name: "ShowInNavigation",
                table: "GroupType");
        }
    }
}
