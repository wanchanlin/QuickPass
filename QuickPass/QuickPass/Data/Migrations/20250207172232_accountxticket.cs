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

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AccountId",
                table: "Tickets",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Accounts_AccountId",
                table: "Tickets",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Accounts_AccountId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_AccountId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Tickets");
        }
    }
}
