using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickPass.Data.Migrations
{
    /// <inheritdoc />
    public partial class VanueTypeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VenueType",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Venue",
                table: "Events");
        }
    }
}
