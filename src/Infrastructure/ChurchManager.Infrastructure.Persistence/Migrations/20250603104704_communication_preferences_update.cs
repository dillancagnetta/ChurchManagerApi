using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class communication_preferences_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUp_Person_AssignedPersonId",
                table: "FollowUp");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUp_Person_AssignedPersonId1",
                table: "FollowUp");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUp_Person_PersonId1",
                table: "FollowUp");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonConnectionHistory_ConnectionStatusType_ConnectionStat~",
                table: "PersonConnectionHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonConnectionHistory_Person_PersonId1",
                table: "PersonConnectionHistory");

            migrationBuilder.DropIndex(
                name: "IX_PersonConnectionHistory_PersonId1",
                table: "PersonConnectionHistory");

            migrationBuilder.DropIndex(
                name: "IX_FollowUp_AssignedPersonId1",
                table: "FollowUp");

            migrationBuilder.DropIndex(
                name: "IX_FollowUp_PersonId1",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "PersonId1",
                table: "PersonConnectionHistory");

            migrationBuilder.DropColumn(
                name: "AssignedPersonId1",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "PersonId1",
                table: "FollowUp");

            migrationBuilder.AlterColumn<int>(
                name: "AssignedPersonId",
                table: "FollowUp",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "CommunicationPreferenceType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultNotSetValue = table.Column<bool>(type: "boolean", nullable: false),
                    CanOverride = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationPreferenceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationPreference",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    PreferenceTypeId = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    CommunicationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationPreference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationPreference_CommunicationPreferenceType_Prefere~",
                        column: x => x.PreferenceTypeId,
                        principalTable: "CommunicationPreferenceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommunicationPreference_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationPreference_PersonId_PreferenceTypeId_Communica~",
                table: "CommunicationPreference",
                columns: new[] { "PersonId", "PreferenceTypeId", "CommunicationType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationPreference_PreferenceTypeId",
                table: "CommunicationPreference",
                column: "PreferenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationPreferenceType_Name",
                table: "CommunicationPreferenceType",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUp_Person_AssignedPersonId",
                table: "FollowUp",
                column: "AssignedPersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonConnectionHistory_ConnectionStatusType_ConnectionStat~",
                table: "PersonConnectionHistory",
                column: "ConnectionStatusTypeId",
                principalTable: "ConnectionStatusType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUp_Person_AssignedPersonId",
                table: "FollowUp");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonConnectionHistory_ConnectionStatusType_ConnectionStat~",
                table: "PersonConnectionHistory");

            migrationBuilder.DropTable(
                name: "CommunicationPreference");

            migrationBuilder.DropTable(
                name: "CommunicationPreferenceType");

            migrationBuilder.AddColumn<int>(
                name: "PersonId1",
                table: "PersonConnectionHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AssignedPersonId",
                table: "FollowUp",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedPersonId1",
                table: "FollowUp",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonId1",
                table: "FollowUp",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonConnectionHistory_PersonId1",
                table: "PersonConnectionHistory",
                column: "PersonId1");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_AssignedPersonId1",
                table: "FollowUp",
                column: "AssignedPersonId1");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_PersonId1",
                table: "FollowUp",
                column: "PersonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUp_Person_AssignedPersonId",
                table: "FollowUp",
                column: "AssignedPersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUp_Person_AssignedPersonId1",
                table: "FollowUp",
                column: "AssignedPersonId1",
                principalTable: "Person",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUp_Person_PersonId1",
                table: "FollowUp",
                column: "PersonId1",
                principalTable: "Person",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonConnectionHistory_ConnectionStatusType_ConnectionStat~",
                table: "PersonConnectionHistory",
                column: "ConnectionStatusTypeId",
                principalTable: "ConnectionStatusType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonConnectionHistory_Person_PersonId1",
                table: "PersonConnectionHistory",
                column: "PersonId1",
                principalTable: "Person",
                principalColumn: "Id");
        }
    }
}
