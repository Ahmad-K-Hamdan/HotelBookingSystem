using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangesToBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_HotelRoom_HotelRoomId",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "NumOfChildren",
                table: "Booking",
                newName: "TotalChildren");

            migrationBuilder.RenameColumn(
                name: "NumOfAdults",
                table: "Booking",
                newName: "TotalAdults");

            migrationBuilder.AlterColumn<Guid>(
                name: "HotelRoomId",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "Nights",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDiscountedPrice",
                table: "Booking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalOriginalPrice",
                table: "Booking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BookingRoom",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HotelRoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumOfAdults = table.Column<int>(type: "int", nullable: false),
                    NumOfChildren = table.Column<int>(type: "int", nullable: false),
                    PricePerNightOriginal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerNightDiscounted = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingRoom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingRoom_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingRoom_HotelRoom_HotelRoomId",
                        column: x => x.HotelRoomId,
                        principalTable: "HotelRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingRoom_BookingId",
                table: "BookingRoom",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRoom_HotelRoomId",
                table: "BookingRoom",
                column: "HotelRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_HotelRoom_HotelRoomId",
                table: "Booking",
                column: "HotelRoomId",
                principalTable: "HotelRoom",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_HotelRoom_HotelRoomId",
                table: "Booking");

            migrationBuilder.DropTable(
                name: "BookingRoom");

            migrationBuilder.DropColumn(
                name: "Nights",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "TotalDiscountedPrice",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "TotalOriginalPrice",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "TotalChildren",
                table: "Booking",
                newName: "NumOfChildren");

            migrationBuilder.RenameColumn(
                name: "TotalAdults",
                table: "Booking",
                newName: "NumOfAdults");

            migrationBuilder.AlterColumn<Guid>(
                name: "HotelRoomId",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_HotelRoom_HotelRoomId",
                table: "Booking",
                column: "HotelRoomId",
                principalTable: "HotelRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
