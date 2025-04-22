using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class events_registrations_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDates_EndDate",
                table: "Event",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDates_StartDate",
                table: "Event",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationDates_EndDate",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "RegistrationDates_StartDate",
                table: "Event");
        }
    }
}
