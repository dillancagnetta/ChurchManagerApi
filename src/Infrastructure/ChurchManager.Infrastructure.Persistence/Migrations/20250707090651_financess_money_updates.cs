using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class financess_money_updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount_Currency",
                schema: "Finances",
                table: "Transaction",
                newName: "TransactionAmount_Currency");

            migrationBuilder.RenameColumn(
                name: "Amount_Amount",
                schema: "Finances",
                table: "Transaction",
                newName: "TransactionAmount_Amount");

            migrationBuilder.RenameColumn(
                name: "Amount_Currency",
                schema: "Finances",
                table: "Giving",
                newName: "GivingAmount_Currency");

            migrationBuilder.RenameColumn(
                name: "Amount_Amount",
                schema: "Finances",
                table: "Giving",
                newName: "GivingAmount_Amount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionAmount_Currency",
                schema: "Finances",
                table: "Transaction",
                newName: "Amount_Currency");

            migrationBuilder.RenameColumn(
                name: "TransactionAmount_Amount",
                schema: "Finances",
                table: "Transaction",
                newName: "Amount_Amount");

            migrationBuilder.RenameColumn(
                name: "GivingAmount_Currency",
                schema: "Finances",
                table: "Giving",
                newName: "Amount_Currency");

            migrationBuilder.RenameColumn(
                name: "GivingAmount_Amount",
                schema: "Finances",
                table: "Giving",
                newName: "Amount_Amount");
        }
    }
}
