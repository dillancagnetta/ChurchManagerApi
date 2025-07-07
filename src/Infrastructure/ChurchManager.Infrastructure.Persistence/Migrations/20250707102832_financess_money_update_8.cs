using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class financess_money_update_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Giving_GivingId",
                schema: "Finances",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_GivingId",
                schema: "Finances",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "GivingId",
                schema: "Finances",
                table: "Transaction");

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

            migrationBuilder.AddColumn<int>(
                name: "BankStatementImportId1",
                schema: "Finances",
                table: "Giving",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Giving_BankStatementImportId1",
                schema: "Finances",
                table: "Giving",
                column: "BankStatementImportId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Giving_BankStatementImport_BankStatementImportId1",
                schema: "Finances",
                table: "Giving",
                column: "BankStatementImportId1",
                principalSchema: "Finances",
                principalTable: "BankStatementImport",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Giving_BankStatementImport_BankStatementImportId1",
                schema: "Finances",
                table: "Giving");

            migrationBuilder.DropIndex(
                name: "IX_Giving_BankStatementImportId1",
                schema: "Finances",
                table: "Giving");

            migrationBuilder.DropColumn(
                name: "BankStatementImportId1",
                schema: "Finances",
                table: "Giving");

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

            migrationBuilder.AddColumn<int>(
                name: "GivingId",
                schema: "Finances",
                table: "Transaction",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_GivingId",
                schema: "Finances",
                table: "Transaction",
                column: "GivingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Giving_GivingId",
                schema: "Finances",
                table: "Transaction",
                column: "GivingId",
                principalSchema: "Finances",
                principalTable: "Giving",
                principalColumn: "Id");
        }
    }
}
