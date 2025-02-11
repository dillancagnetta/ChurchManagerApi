using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Communications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttemptCount",
                table: "CommunicationRecipient",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CommunicationContent",
                table: "Communication",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Communication",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRecipient_PersonId",
                table: "CommunicationRecipient",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunicationRecipient_Person_PersonId",
                table: "CommunicationRecipient",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunicationRecipient_Person_PersonId",
                table: "CommunicationRecipient");

            migrationBuilder.DropIndex(
                name: "IX_CommunicationRecipient_PersonId",
                table: "CommunicationRecipient");

            migrationBuilder.DropColumn(
                name: "AttemptCount",
                table: "CommunicationRecipient");

            migrationBuilder.DropColumn(
                name: "CommunicationContent",
                table: "Communication");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Communication");
        }
    }
}
