using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Cascade_delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Group_ChildCareGroupId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Group_EventRegistrationGroupId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Person_ContactPersonId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Church_ChurchId",
                table: "Person");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Group_ChildCareGroupId",
                table: "Event",
                column: "ChildCareGroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Group_EventRegistrationGroupId",
                table: "Event",
                column: "EventRegistrationGroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Person_ContactPersonId",
                table: "Event",
                column: "ContactPersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Church_ChurchId",
                table: "Person",
                column: "ChurchId",
                principalTable: "Church",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Group_ChildCareGroupId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Group_EventRegistrationGroupId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Person_ContactPersonId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Church_ChurchId",
                table: "Person");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Group_ChildCareGroupId",
                table: "Event",
                column: "ChildCareGroupId",
                principalTable: "Group",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Group_EventRegistrationGroupId",
                table: "Event",
                column: "EventRegistrationGroupId",
                principalTable: "Group",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Person_ContactPersonId",
                table: "Event",
                column: "ContactPersonId",
                principalTable: "Person",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Church_ChurchId",
                table: "Person",
                column: "ChurchId",
                principalTable: "Church",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
