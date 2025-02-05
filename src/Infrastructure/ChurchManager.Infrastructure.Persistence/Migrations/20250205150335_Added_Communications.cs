using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Communications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunicationTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    LogoFileUrl = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    SupportedTypes = table.Column<string>(type: "text", nullable: true),
                    IsBaseTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemCommunication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemCommunication", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CommunicationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ListGroupId = table.Column<int>(type: "integer", nullable: true),
                    CommunicationTemplateId = table.Column<int>(type: "integer", nullable: true),
                    SenderPersonId = table.Column<int>(type: "integer", nullable: true),
                    IsBulkCommunication = table.Column<bool>(type: "boolean", nullable: false),
                    SendDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    FutureSendDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Review_ReviewerNote = table.Column<string>(type: "text", nullable: true),
                    Review_ReviewedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Review_ReviewerPersonId = table.Column<int>(type: "integer", nullable: true),
                    Metadata = table.Column<string>(type: "jsonb", nullable: true),
                    SystemCommunicationId = table.Column<int>(type: "integer", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communication_CommunicationTemplate_CommunicationTemplateId",
                        column: x => x.CommunicationTemplateId,
                        principalTable: "CommunicationTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Communication_Group_ListGroupId",
                        column: x => x.ListGroupId,
                        principalTable: "Group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Communication_Person_SenderPersonId",
                        column: x => x.SenderPersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Communication_SystemCommunication_SystemCommunicationId",
                        column: x => x.SystemCommunicationId,
                        principalTable: "SystemCommunication",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommunicationAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommunicationId = table.Column<int>(type: "integer", nullable: false),
                    CommunicationType = table.Column<string>(type: "text", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    MimeType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    FileUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FileContents = table.Column<string>(type: "text", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationAttachment_Communication_CommunicationId",
                        column: x => x.CommunicationId,
                        principalTable: "Communication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationRecipient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommunicationId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    StatusNote = table.Column<string>(type: "text", nullable: true),
                    SendDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    OpenedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UniqueMessageId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationRecipient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationRecipient_Communication_CommunicationId",
                        column: x => x.CommunicationId,
                        principalTable: "Communication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Communication_CommunicationTemplateId",
                table: "Communication",
                column: "CommunicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_ListGroupId",
                table: "Communication",
                column: "ListGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_SenderPersonId",
                table: "Communication",
                column: "SenderPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Communication_SystemCommunicationId",
                table: "Communication",
                column: "SystemCommunicationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationAttachment_CommunicationId",
                table: "CommunicationAttachment",
                column: "CommunicationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationRecipient_CommunicationId",
                table: "CommunicationRecipient",
                column: "CommunicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunicationAttachment");

            migrationBuilder.DropTable(
                name: "CommunicationRecipient");

            migrationBuilder.DropTable(
                name: "Communication");

            migrationBuilder.DropTable(
                name: "CommunicationTemplate");

            migrationBuilder.DropTable(
                name: "SystemCommunication");
        }
    }
}
