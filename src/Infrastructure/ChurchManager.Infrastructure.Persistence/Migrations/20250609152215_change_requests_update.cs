using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class change_requests_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChangeRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequestedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    Source = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ReviewedByPersonId = table.Column<int>(type: "integer", nullable: true),
                    ReviewedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReviewNotes = table.Column<string>(type: "text", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeRequest_Person_ReviewedByPersonId",
                        column: x => x.ReviewedByPersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PropertyChangeRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChangeRequestId = table.Column<int>(type: "integer", nullable: false),
                    EntityType = table.Column<string>(type: "text", nullable: false),
                    EntityId = table.Column<int>(type: "integer", nullable: false),
                    PropertyPath = table.Column<string>(type: "text", nullable: false),
                    CurrentValue = table.Column<string>(type: "text", nullable: true),
                    RequestedValue = table.Column<string>(type: "text", nullable: true),
                    PropertyType = table.Column<string>(type: "text", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyChangeRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyChangeRequest_ChangeRequest_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequest_ReviewedByPersonId",
                table: "ChangeRequest",
                column: "ReviewedByPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyChangeRequest_ChangeRequestId",
                table: "PropertyChangeRequest",
                column: "ChangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyChangeRequest_EntityId",
                table: "PropertyChangeRequest",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyChangeRequest_EntityType",
                table: "PropertyChangeRequest",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyChangeRequest_PropertyPath",
                table: "PropertyChangeRequest",
                column: "PropertyPath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyChangeRequest");

            migrationBuilder.DropTable(
                name: "ChangeRequest");
        }
    }
}
