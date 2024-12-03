using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LinkChurchToPersonFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_History_RelatedEntityId",
                table: "History",
                column: "RelatedEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_History_RelatedEntityType",
                table: "History",
                column: "RelatedEntityType");

            migrationBuilder.CreateIndex(
                name: "IX_ChurchGroup_LeaderPersonId",
                table: "ChurchGroup",
                column: "LeaderPersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Church_LeaderPersonId",
                table: "Church",
                column: "LeaderPersonId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Church_Person_LeaderPersonId",
                table: "Church",
                column: "LeaderPersonId",
                principalTable: "Person",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChurchGroup_Person_LeaderPersonId",
                table: "ChurchGroup",
                column: "LeaderPersonId",
                principalTable: "Person",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Church_Person_LeaderPersonId",
                table: "Church");

            migrationBuilder.DropForeignKey(
                name: "FK_ChurchGroup_Person_LeaderPersonId",
                table: "ChurchGroup");

            migrationBuilder.DropIndex(
                name: "IX_History_RelatedEntityId",
                table: "History");

            migrationBuilder.DropIndex(
                name: "IX_History_RelatedEntityType",
                table: "History");

            migrationBuilder.DropIndex(
                name: "IX_ChurchGroup_LeaderPersonId",
                table: "ChurchGroup");

            migrationBuilder.DropIndex(
                name: "IX_Church_LeaderPersonId",
                table: "Church");
        }
    }
}
