using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CheckInCheckOutToDateOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckInDateTime",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CheckOutDateTime",
                table: "Booking");

            migrationBuilder.AddColumn<DateOnly>(
                name: "CheckInDate",
                table: "Booking",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "CheckOutDate",
                table: "Booking",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_City_CityName_CountryName",
                table: "City",
                columns: new[] { "CityName", "CountryName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_City_CityName_CountryName",
                table: "City");

            migrationBuilder.DropColumn(
                name: "CheckInDate",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CheckOutDate",
                table: "Booking");

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckInDateTime",
                table: "Booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOutDateTime",
                table: "Booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
