using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_ServiceJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:hstore", ",,");

            migrationBuilder.CreateTable(
                name: "ServiceJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobKey = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Assembly = table.Column<string>(type: "character varying(260)", maxLength: 260, nullable: true),
                    Class = table.Column<string>(type: "character varying(260)", maxLength: 260, nullable: false),
                    CronExpression = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    LastRunDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastRunDurationSeconds = table.Column<int>(type: "integer", nullable: true),
                    LastStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastStatusMessage = table.Column<string>(type: "text", nullable: true),
                    JobParameters = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    NotificationEmails = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    NotificationStatus = table.Column<int>(type: "integer", nullable: false),
                    EnableHistory = table.Column<bool>(type: "boolean", nullable: false),
                    HistoryCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceJobHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceJobId = table.Column<int>(type: "integer", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    StopDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StatusMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceJobHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceJobHistory_ServiceJobs_ServiceJobId",
                        column: x => x.ServiceJobId,
                        principalTable: "ServiceJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceJobHistory_ServiceJobId",
                table: "ServiceJobHistory",
                column: "ServiceJobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceJobHistory");

            migrationBuilder.DropTable(
                name: "ServiceJobs");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:hstore", ",,");
        }
    }
}
