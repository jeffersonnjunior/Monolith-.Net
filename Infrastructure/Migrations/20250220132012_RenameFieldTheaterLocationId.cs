using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameFieldTheaterLocationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieTheaters_TheaterLocation_AddressId",
                table: "MovieTheaters");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "MovieTheaters",
                newName: "TheaterLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieTheaters_AddressId",
                table: "MovieTheaters",
                newName: "IX_MovieTheaters_TheaterLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieTheaters_TheaterLocation_TheaterLocationId",
                table: "MovieTheaters",
                column: "TheaterLocationId",
                principalTable: "TheaterLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieTheaters_TheaterLocation_TheaterLocationId",
                table: "MovieTheaters");

            migrationBuilder.RenameColumn(
                name: "TheaterLocationId",
                table: "MovieTheaters",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieTheaters_TheaterLocationId",
                table: "MovieTheaters",
                newName: "IX_MovieTheaters_AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieTheaters_TheaterLocation_AddressId",
                table: "MovieTheaters",
                column: "AddressId",
                principalTable: "TheaterLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
