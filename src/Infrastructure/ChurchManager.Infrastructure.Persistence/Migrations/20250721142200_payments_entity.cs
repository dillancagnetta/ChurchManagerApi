using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class payments_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "Finances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PaymentMethodSystemName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PaymentReference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    InitiatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PaidAmount_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    PaidAmount_Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    RefundedAmount_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    RefundedAmount_Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Fee_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    Fee_Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    NetAmount_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    NetAmount_Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PaymentId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ExternalReferenceId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Error = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ConvertedToGiving = table.Column<bool>(type: "boolean", nullable: false),
                    GivingId = table.Column<int>(type: "integer", nullable: true),
                    ChurchId = table.Column<int>(type: "integer", nullable: true),
                    PersonId = table.Column<int>(type: "integer", nullable: true),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Church_ChurchId",
                        column: x => x.ChurchId,
                        principalSchema: "Churches",
                        principalTable: "Church",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Payments_Giving_GivingId",
                        column: x => x.GivingId,
                        principalSchema: "Finances",
                        principalTable: "Giving",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ChurchId",
                schema: "Finances",
                table: "Payments",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_GivingId",
                schema: "Finances",
                table: "Payments",
                column: "GivingId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentId",
                schema: "Finances",
                table: "Payments",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethod",
                schema: "Finances",
                table: "Payments",
                column: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethodSystemName",
                schema: "Finances",
                table: "Payments",
                column: "PaymentMethodSystemName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments",
                schema: "Finances");
        }
    }
}
