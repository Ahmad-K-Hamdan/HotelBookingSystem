using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoreChangesToBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_HotelRoom_HotelRoomId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_HotelRoomId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "HotelRoomId",
                table: "Booking");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HotelRoomId",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_HotelRoomId",
                table: "Booking",
                column: "HotelRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_HotelRoom_HotelRoomId",
                table: "Booking",
                column: "HotelRoomId",
                principalTable: "HotelRoom",
                principalColumn: "Id");
        }
    }
}
