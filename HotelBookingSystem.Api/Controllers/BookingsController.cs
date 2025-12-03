using HotelBookingSystem.Application.Features.Bookings.Commands.CreateBooking;
using HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingDetailsById;
using HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingDetailsById.Dtos;
using HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingInvoicePdf;
using HotelBookingSystem.Application.Features.Bookings.Queries.GetMyBookings;
using HotelBookingSystem.Application.Features.Payments.Commands.CreatePaymentForBooking;
using HotelBookingSystem.Application.Features.Payments.Queries.GetPaymentsForBooking;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing bookings and their payments.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the BookingsController class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// [Authenticated] Creates a new booking for the current guest.
    /// </summary>
    /// <remarks>
    /// This endpoint is used from the Secure Checkout flow to confirm a booking.
    ///
    /// The booking is always created for the **currently authenticated user**,
    /// who must already have a Guest profile in the system.
    ///
    /// **Request payload (`CreateBookingCommand`):**
    /// - `CheckInDate`, `CheckOutDate` – required; `CheckOutDate` must be after `CheckInDate`.
    /// - `Rooms` – a list of room requests; each item contains:
    ///   - `HotelRoomTypeId` – the room type to book.
    ///   - `Adults` – number of adults in this room (must be &gt; 0).
    ///   - `Children` – number of children in this room (≥ 0).
    /// - `SpecialRequests` – optional free-text notes for the hotel.
    ///
    /// **Business rules:**
    /// - All `HotelRoomTypeId` values must belong to the same hotel.
    /// - For each requested room:
    ///   - There must be at least one **available** room of that type for the given dates.
    ///   - Occupancy (`Adults`/`Children`) must not exceed the room type capacity.
    /// - Prices are calculated from the room type price, the number of nights,
    ///   and any active discount on the hotel.
    ///
    /// On success, the service:
    /// - Creates a `Booking` record with a generated `ConfirmationCode`.
    /// - Creates one or more `BookingRoom` records, each linked to a concrete `HotelRoom`.
    /// </remarks>
    /// <param name="command">The command containing booking creation details.</param>
    /// <returns>The ID of the newly created booking.</returns>
    /// <response code="201">Booking was successfully created.</response>
    /// <response code="400">Validation failed or some room types are not available.</response>
    /// <response code="401">User is not authenticated or has no guest profile.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetBookingDetailsById), new { id }, id);
    }

    /// <summary>
    /// [Authenticated] Retrieves a detailed view of a specific booking for the current guest.
    /// </summary>
    /// <remarks>
    /// This endpoint returns a rich view of a booking, including:
    ///
    /// **Booking details:**
    /// - Dates: `CheckInDate`, `CheckOutDate`, `Nights`.
    /// - Total occupants: `TotalAdults`, `TotalChildren`.
    /// - `ConfirmationCode`, `SpecialRequests`, `CreatedAt`.
    /// - `TotalOriginalPrice`, `TotalDiscountedPrice`.
    ///
    /// **Hotel summary:**
    /// - `HotelId`, `HotelName`, `CityName`, `CountryName`, `StarRating`.
    ///
    /// **Guest summary:**
    /// - `GuestId`, `GuestHomeCountry`.
    ///
    /// **Booked rooms (`BookingRoomDto` list):**
    /// - `BookingRoomId`, `HotelRoomId`.
    /// - `RoomTypeName`, `RoomNumber`.
    /// - Occupancy per room: `Adults`, `Children`.
    /// - `PricePerNightOriginal`, `PricePerNightDiscounted`.
    ///
    /// Only the owner of the booking (same authenticated user) is allowed
    /// to view its details.
    /// </remarks>
    /// <param name="id">The ID of the booking to retrieve.</param>
    /// <returns>The details of the requested booking.</returns>
    /// <response code="200">Successfully returned the booking details.</response>
    /// <response code="404">No booking was found with the given ID.</response>
    /// <response code="401">User is not authenticated or does not own this booking.</response>
    [HttpGet("{id:guid}/details")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(BookingDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetBookingDetailsById(Guid id)
        => Ok(await _mediator.Send(new GetBookingDetailsByIdQuery(id)));

    /// <summary>
    /// [Authenticated] Retrieves all bookings for the current guest.
    /// </summary>
    /// <remarks>
    /// This endpoint is typically used in a "My Bookings" page.
    ///
    /// **Query parameters:**
    /// - `UpcomingOnly` – if true, returns only bookings where `CheckInDate` is today or later.
    /// - `PastOnly` – if true, returns only bookings where `CheckOutDate` is before today.
    ///   If both flags are false, all bookings are returned.
    ///
    /// Each `BookingDto` includes:
    /// - `Id`, `HotelName`, `CityName`.
    /// - `CheckInDate`, `CheckOutDate`, `Nights`.
    /// - `ConfirmationCode`.
    /// - `TotalOriginalPrice`, `TotalDiscountedPrice`.
    /// </remarks>
    /// <param name="query">Filter options for the bookings list.</param>
    /// <returns>A list of bookings for the current guest.</returns>
    /// <response code="200">Successfully returned the list of bookings.</response>
    /// <response code="401">User is not authenticated.</response>
    [HttpGet("my")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyBookings([FromQuery] GetMyBookingsQuery query)
        => Ok(await _mediator.Send(query));

    /// <summary>
    /// [Authenticated] Records a payment for a specific booking.
    /// </summary>
    /// <remarks>
    /// This endpoint is part of the checkout / payment flow.
    ///
    /// **Important notes:**
    /// - The booking must belong to the current authenticated guest.
    /// - The payment amount can be partial – multiple payments may exist for a single booking.
    /// - The handler ensures:
    ///   - The booking exists and is owned by the current user.
    ///   - The payment method exists.
    ///   - The new payment does not exceed the remaining amount
    ///     (`TotalDiscountedPrice - sum(Completed payments)`).
    ///
    /// **Request payload (`CreatePaymentForBookingCommand`):**
    /// - `BookingId` – the booking to pay for.
    /// - `PaymentMethodId` – the chosen payment method.
    /// - `Amount` – the amount to be recorded for this payment.
    ///
    /// On success, a new `Payment` entity is created with status `Completed`.
    /// </remarks>
    /// <param name="bookingId">The booking ID, taken from the route.</param>
    /// <param name="command">The command containing payment details.</param>
    /// <returns>The ID of the newly created payment.</returns>
    /// <response code="201">Payment was successfully recorded.</response>
    /// <response code="400">Validation failed or amount exceeds remaining due.</response>
    /// <response code="401">User is not authenticated or does not own this booking.</response>
    /// <response code="404">Booking or payment method not found.</response>
    [HttpPost("{bookingId:guid}/payments")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreatePaymentForBooking(Guid bookingId, [FromBody] CreatePaymentForBookingCommand command)
    {
        command.BookingId = bookingId;
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetPaymentsForBooking), new { bookingId }, id);
    }

    /// <summary>
    /// [Authenticated] Retrieves all payments recorded for a specific booking.
    /// </summary>
    /// <remarks>
    /// Only the owner of the booking may view its payments.
    ///
    /// Each `PaymentDto` includes:
    /// - `Id`
    /// - `PaymentAmount`
    /// - `PaymentStatus`
    /// - `PaymentDate`
    /// - `PaymentMethodId`
    /// </remarks>
    /// <param name="bookingId">The ID of the booking.</param>
    /// <returns>A list of payments associated with the booking.</returns>
    /// <response code="200">Successfully returned the list of payments.</response>
    /// <response code="401">User is not authenticated or does not own this booking.</response>
    /// <response code="404">Booking not found.</response>
    [HttpGet("{bookingId:guid}/payments")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<PaymentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPaymentsForBooking(Guid bookingId)
        => Ok(await _mediator.Send(new GetPaymentsForBookingQuery(bookingId)));

    /// <summary>
    /// [Authenticated] Downloads the booking invoice as a PDF file for printing or saving.
    /// </summary>
    /// <remarks>
    /// This endpoint is intended for the Booking Confirmation page.
    /// It generates a PDF with booking and pricing details identical to the confirmation view.
    /// </remarks>
    /// <param name="id">The booking ID.</param>
    /// <returns>PDF file containing the booking invoice.</returns>
    /// <response code="200">PDF generated successfully.</response>
    /// <response code="404">Booking not found.</response>
    [HttpGet("{id:guid}/invoice")]
    [Authorize]
    [Produces("application/pdf")]
    public async Task<IActionResult> DownloadInvoice(Guid id)
    {
        var pdfBytes = await _mediator.Send(new GetBookingInvoicePdfQuery(id));
        return File(pdfBytes, "application/pdf", $"booking-{id}-invoice.pdf");
    }
}
