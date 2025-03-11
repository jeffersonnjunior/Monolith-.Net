using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameFieldCustomerDetailsId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_CustomerDetails_ClientId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "FilmAudioOption",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "FilmFormat",
                table: "Films");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Tickets",
                newName: "CustomerDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_ClientId",
                table: "Tickets",
                newName: "IX_Tickets_CustomerDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_CustomerDetails_CustomerDetailsId",
                table: "Tickets",
                column: "CustomerDetailsId",
                principalTable: "CustomerDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_CustomerDetails_CustomerDetailsId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "CustomerDetailsId",
                table: "Tickets",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_CustomerDetailsId",
                table: "Tickets",
                newName: "IX_Tickets_ClientId");

            migrationBuilder.AddColumn<int>(
                name: "FilmAudioOption",
                table: "Films",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FilmFormat",
                table: "Films",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_CustomerDetails_ClientId",
                table: "Tickets",
                column: "ClientId",
                principalTable: "CustomerDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
