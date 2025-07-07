using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class financess_money_update_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionAmount_Currency",
                schema: "Finances",
                table: "Transaction",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "TransactionAmount_Amount",
                schema: "Finances",
                table: "Transaction",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "GivingAmount_Currency",
                schema: "Finances",
                table: "Giving",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "GivingAmount_Amount",
                schema: "Finances",
                table: "Giving",
                newName: "Amount");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                schema: "Finances",
                table: "Transaction",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "Finances",
                table: "Transaction",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                schema: "Finances",
                table: "Giving",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "Finances",
                table: "Giving",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                schema: "Finances",
                table: "Transaction",
                newName: "TransactionAmount_Currency");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "Finances",
                table: "Transaction",
                newName: "TransactionAmount_Amount");

            migrationBuilder.RenameColumn(
                name: "Currency",
                schema: "Finances",
                table: "Giving",
                newName: "GivingAmount_Currency");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "Finances",
                table: "Giving",
                newName: "GivingAmount_Amount");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionAmount_Currency",
                schema: "Finances",
                table: "Transaction",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TransactionAmount_Amount",
                schema: "Finances",
                table: "Transaction",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "GivingAmount_Currency",
                schema: "Finances",
                table: "Giving",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "GivingAmount_Amount",
                schema: "Finances",
                table: "Giving",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }
    }
}
