using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "UserLogin");

            migrationBuilder.CreateTable(
                name: "UserLoginRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    UserLoginId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLoginRole_UserLogin_UserLoginId",
                        column: x => x.UserLoginId,
                        principalTable: "UserLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityPermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserLoginRoleId = table.Column<int>(type: "integer", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    EntityType = table.Column<string>(type: "text", nullable: true),
                    EntityIds = table.Column<int[]>(type: "integer[]", nullable: true),
                    IsDynamicScope = table.Column<bool>(type: "boolean", nullable: false),
                    ScopeType = table.Column<string>(type: "text", nullable: true),
                    ScopeId = table.Column<int>(type: "integer", nullable: false),
                    CanView = table.Column<bool>(type: "boolean", nullable: false),
                    CanEdit = table.Column<bool>(type: "boolean", nullable: false),
                    CanDelete = table.Column<bool>(type: "boolean", nullable: false),
                    CanManageUsers = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    InactiveDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityPermission_UserLoginRole_UserLoginRoleId",
                        column: x => x.UserLoginRoleId,
                        principalTable: "UserLoginRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityPermission_UserLoginRoleId",
                table: "EntityPermission",
                column: "UserLoginRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginRole_UserLoginId",
                table: "UserLoginRole",
                column: "UserLoginId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityPermission");

            migrationBuilder.DropTable(
                name: "UserLoginRole");

            migrationBuilder.AddColumn<List<string>>(
                name: "Roles",
                table: "UserLogin",
                type: "text[]",
                nullable: true);
        }
    }
}
