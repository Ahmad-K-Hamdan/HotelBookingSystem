using System.Globalization;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingDetailsById.Dtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HotelBookingSystem.Infrastructure.Services
{
    public class BookingInvoicePdfGenerator : IBookingInvoicePdfGenerator
    {
        public byte[] Generate(BookingDetailsDto booking)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Booking Invoice")
                                .FontSize(20)
                                .SemiBold();

                            col.Item().Text($"Confirmation Code: {booking.ConfirmationCode}");
                            col.Item().Text($"Created At: {booking.CreatedAt:yyyy-MM-dd HH:mm}");
                        });

                        row.ConstantItem(250).Column(col =>
                        {
                            col.Item().Text(booking.HotelName).Bold();
                            col.Item().Text(booking.HotelAddress);
                            col.Item().Text($"{booking.CityName}, {booking.CountryName}");
                            col.Item().Text($"{booking.StarRating}★");
                        });
                    });

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text("Guest Details").Bold();
                        col.Item().Text($"Name: {booking.GuestFullName}");
                        col.Item().Text($"Home Country: {booking.GuestHomeCountry}");
                        col.Item().Text($"Passport Number: {booking.GuestPassportNumber}");

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        col.Item().Text("Stay Details").Bold();
                        col.Item().Text($"Check-in: {booking.CheckInDate:yyyy-MM-dd}");
                        col.Item().Text($"Check-out: {booking.CheckOutDate:yyyy-MM-dd}");
                        col.Item().Text($"Nights: {booking.Nights}");
                        col.Item().Text($"Guests: {booking.TotalAdults} adults, {booking.TotalChildren} children");

                        if (!string.IsNullOrWhiteSpace(booking.SpecialRequests))
                        {
                            col.Item().Text($"Special Requests: {booking.SpecialRequests}");
                        }

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        col.Item().Text("Rooms").Bold();

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCell).Text("Room Type");
                                header.Cell().Element(HeaderCell).Text("Guests");
                                header.Cell().Element(HeaderCell).Text("Price/Night");
                                header.Cell().Element(HeaderCell).Text("Total");
                            });

                            foreach (var room in booking.Rooms)
                            {
                                table.Cell().Element(CellStyle).Text(room.RoomTypeName);
                                table.Cell().Element(CellStyle).Text($"{room.Adults} + {room.Children}");
                                table.Cell().Element(CellStyle).Text(room.PricePerNightDiscounted.ToString("C", CultureInfo.CurrentCulture));
                                table.Cell().Element(CellStyle).Text(booking.TotalDiscountedPrice.ToString("C", CultureInfo.CurrentCulture));
                            }

                            IContainer HeaderCell(IContainer container) =>
                                container.DefaultTextStyle(x => x.SemiBold()).Padding(4);

                            IContainer CellStyle(IContainer container) =>
                                container.Padding(4);
                        });

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem();
                            row.ConstantItem(250).Column(summary =>
                            {
                                summary.Spacing(3);
                                summary.Item().Text($"Original Total: {booking.TotalOriginalPrice:C}");
                                summary.Item().Text($"Discounted Total: {booking.TotalDiscountedPrice:C}")
                                    .Bold();
                            });
                        });
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("Generated on ").FontSize(9);
                        txt.Span(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm 'UTC'")).FontSize(9);
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
