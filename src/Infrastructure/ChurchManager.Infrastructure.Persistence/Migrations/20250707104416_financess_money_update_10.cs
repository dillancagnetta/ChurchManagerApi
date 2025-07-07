using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class financess_money_update_10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
