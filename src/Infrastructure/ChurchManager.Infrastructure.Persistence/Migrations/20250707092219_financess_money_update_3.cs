using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class financess_money_update_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Offering_Currency",
                schema: "Missions",
                table: "Mission",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "Offering_Amount",
                schema: "Missions",
                table: "Mission",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Offering_Currency",
                schema: "Groups",
                table: "GroupAttendance",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "Offering_Amount",
                schema: "Groups",
                table: "GroupAttendance",
                newName: "Amount");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                schema: "Missions",
                table: "Mission",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "Missions",
                table: "Mission",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                schema: "Groups",
                table: "GroupAttendance",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "Groups",
                table: "GroupAttendance",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                schema: "Missions",
                table: "Mission",
                newName: "Offering_Currency");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "Missions",
                table: "Mission",
                newName: "Offering_Amount");

            migrationBuilder.RenameColumn(
                name: "Currency",
                schema: "Groups",
                table: "GroupAttendance",
                newName: "Offering_Currency");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "Groups",
                table: "GroupAttendance",
                newName: "Offering_Amount");

            migrationBuilder.AlterColumn<string>(
                name: "Offering_Currency",
                schema: "Missions",
                table: "Mission",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Offering_Amount",
                schema: "Missions",
                table: "Mission",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Offering_Currency",
                schema: "Groups",
                table: "GroupAttendance",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Offering_Amount",
                schema: "Groups",
                table: "GroupAttendance",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
