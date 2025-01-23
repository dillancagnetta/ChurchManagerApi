﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_FamilyCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Family",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Family");
        }
    }
}
