using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickPass.Data.Migrations
{
    /// <inheritdoc />
    public partial class accountxticket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicektID",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Events_AccountId",
                table: "Events",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Accounts_AccountId",
                table: "Events",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Accounts_AccountId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_AccountId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TicektID",
                table: "Accounts");
        }
    }
}
