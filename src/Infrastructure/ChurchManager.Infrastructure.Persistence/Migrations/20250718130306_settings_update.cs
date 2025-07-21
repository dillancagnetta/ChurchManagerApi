using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class settings_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setting_ChurchGroup_ChurchGroupId",
                schema: "Common",
                table: "Setting");

            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Church_ChurchId",
                schema: "Common",
                table: "Setting");

            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Family_FamilyId",
                schema: "Common",
                table: "Setting");

            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Person_PersonId",
                schema: "Common",
                table: "Setting");

            migrationBuilder.DropIndex(
                name: "IX_Setting_ChurchGroupId",
                schema: "Common",
                table: "Setting");

            migrationBuilder.DropIndex(
                name: "IX_Setting_ChurchId",
                schema: "Common",
                table: "Setting");

            migrationBuilder.DropIndex(
                name: "IX_Setting_FamilyId",
                schema: "Common",
                table: "Setting");

            migrationBuilder.DropIndex(
                name: "IX_Setting_PersonId",
                schema: "Common",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "ChurchGroupId",
                schema: "Common",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "ChurchId",
                schema: "Common",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "FamilyId",
                schema: "Common",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "PersonId",
                schema: "Common",
                table: "Setting");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChurchGroupId",
                schema: "Common",
                table: "Setting",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChurchId",
                schema: "Common",
                table: "Setting",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FamilyId",
                schema: "Common",
                table: "Setting",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                schema: "Common",
                table: "Setting",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Setting_ChurchGroupId",
                schema: "Common",
                table: "Setting",
                column: "ChurchGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_ChurchId",
                schema: "Common",
                table: "Setting",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_FamilyId",
                schema: "Common",
                table: "Setting",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_PersonId",
                schema: "Common",
                table: "Setting",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_ChurchGroup_ChurchGroupId",
                schema: "Common",
                table: "Setting",
                column: "ChurchGroupId",
                principalSchema: "Churches",
                principalTable: "ChurchGroup",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Church_ChurchId",
                schema: "Common",
                table: "Setting",
                column: "ChurchId",
                principalSchema: "Churches",
                principalTable: "Church",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Family_FamilyId",
                schema: "Common",
                table: "Setting",
                column: "FamilyId",
                principalSchema: "People",
                principalTable: "Family",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Person_PersonId",
                schema: "Common",
                table: "Setting",
                column: "PersonId",
                principalSchema: "People",
                principalTable: "Person",
                principalColumn: "Id");
        }
    }
}
