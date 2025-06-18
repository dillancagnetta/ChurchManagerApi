using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class communication_preferences_update_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultCommunicationType",
                table: "CommunicationPreferenceType",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultCommunicationType",
                table: "CommunicationPreferenceType");
        }
    }
}
