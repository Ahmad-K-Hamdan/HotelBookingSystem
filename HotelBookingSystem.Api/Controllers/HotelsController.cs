using HotelBookingSystem.Application.Features.Hotels.Commands.CreateHotel;
using HotelBookingSystem.Application.Features.Hotels.Commands.DeleteHotel;
using HotelBookingSystem.Application.Features.Hotels.Commands.UpdateHotel;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelById;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelById.Dtos;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing hotel related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the HotelsController class.
    /// </summary>   
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public HotelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves hotels using advanced filtering and room-matching logic.
    /// </summary>
    /// <remarks>
    /// This endpoint is intended for the main search on the Home / Search Results page.
    ///
    /// **Important query parameters:**
    /// - `CheckInDate`, `CheckOutDate` – required; default to today/tomorrow if not provided.
    /// - `Rooms` – one or more room requests (adults/children per room).
    /// - `CityName`, `CountryName` – optional location filters.
    /// - `MinStars` – minimum star rating (1–5).
    /// - `MinPrice`, `MaxPrice` – price range (based on discounted per-night cost).
    /// - `OnlyWithActiveDiscount` – return only hotels with active discounts.
    /// - `AmenityIds`, `RoomTypes` – filter by amenities or specific room type names.
    /// - `Sort` – options: `price`, `price desc`, `stars`, `stars desc`, `most visited`.
    /// - `Limit` – optional maximum number of hotels to return (e.g., top 5).
    ///
    /// **Returned `HotelDto` includes:**
    /// - Hotel info: name, address, city, country, star rating, and hotel group.
    /// - `HasActiveDiscount` and `VisitCount`.
    /// - Effective pricing based on matched rooms: `MinOriginalPricePerNight` and `MinDiscountedPricePerNight`.
    /// - `MainImageUrl` and a list of amenity names.
    /// - A list of `MatchedRoomDto` – the actual rooms chosen for the user’s occupancy request,
    ///   with per-room original and discounted prices.
    /// </remarks>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<HotelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetHotels([FromQuery] GetHotelsQuery getHotelsQuery)
        => Ok(await _mediator.Send(getHotelsQuery));

    /// <summary>
    /// Creates a new hotel in the system.
    /// </summary>
    /// <remarks>
    /// This endpoint is used by the Admin interface to register a new hotel.
    ///
    /// **Required fields in `CreateHotelDto`:**
    /// - `HotelGroupId` — must reference an existing hotel group.
    /// - `CityId` — the ID of the city where the hotel is located.
    /// - `HotelName` and `HotelAddress` — the hotel's identity and physical location.
    /// - `StarRating`, `Latitude`, `Longitude`, and `Description` — essential rating and geo information.
    ///
    /// Validation is handled using FluentValidation.  
    /// On success, the API returns the **GUID** identifier of the newly created hotel.
    /// </remarks>
    /// <param name="createHotelDto">The data required to create a new hotel.</param>
    /// <returns>The ID of the newly created hotel.</returns>
    /// <response code="200">Hotel was successfully created and the new ID was returned.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDto createHotelDto)
    {
        var id = await _mediator.Send(new CreateHotelCommand(createHotelDto));
        return CreatedAtAction(nameof(GetHotelById), new { id }, id);
    }

    /// <summary>
    /// Retrieves detailed information about a specific hotel.
    /// </summary>
    /// <remarks>
    /// This endpoint is used for the **Hotel Details Page** on the client application.
    ///
    /// The response includes:
    /// - Hotel info: name, address, city, country, star rating, hotel group, description.
    /// - List of hotel images.
    /// - Amenity names.
    /// - Room types with capacity, pricing (original + discounted), availability per type, and room-type images.
    /// - Reviews, average rating, and total review count.
    ///
    /// **Date range:**
    /// - `checkInDate` and `checkOutDate` determine room-type availability and pricing.
    /// - If not provided, defaults are applied: **today → tomorrow**.
    ///
    /// **Availability logic:**
    /// - A room type is marked as available if **any** room under that type is free across the entire date range.
    /// - Individual room numbers are intentionally not returned.
    /// </remarks>
    /// <param name="id">The ID of the hotel to retrieve.</param>
    /// <param name="checkInDate">Optional check-in date (defaults to today).</param>
    /// <param name="checkOutDate">Optional check-out date (defaults to tomorrow).</param>
    /// <returns>Detailed information for the requested hotel.</returns>
    /// <response code="200">Successfully returned the hotel details.</response>
    /// <response code="404">No hotel was found with the given ID.</response>
    /// <response code="400">Invalid request, such as an invalid date range.</response>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(HotelDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetHotelById(Guid id, [FromQuery] DateOnly? checkInDate, [FromQuery] DateOnly? checkOutDate)
    {
        return Ok(await _mediator.Send(new GetHotelByIdQuery(id, checkInDate, checkOutDate)));
    }

    /// <summary>
    /// Updates an existing hotel in the system.
    /// </summary>
    /// <remarks>
    /// This endpoint is used by the Admin interface to modify hotel information.
    ///
    /// The request body must contain a valid <c>UpdateHotelCommand</c>, which includes:
    /// - <c>Id</c> — must match the hotel ID in the route.
    /// - <c>HotelName</c>, <c>HotelAddress</c>, <c>StarRating</c>, <c>Latitude</c>, <c>Longitude</c>, etc.
    /// - <c>HotelGroupId</c> and <c>CityId</c> must reference existing entities.
    ///
    /// **Validation:**
    /// - If the route ID does not match <c>command.Id</c>, the request is rejected.
    /// - FluentValidation ensures that all required fields are valid.
    ///
    /// On success, no content is returned.
    /// </remarks>
    /// <param name="id">The ID of the hotel to update.</param>
    /// <param name="command">The updated hotel information.</param>
    /// <response code="204">Hotel was successfully updated.</response>
    /// <response code="404">No hotel was found with the given ID.</response>
    /// <response code="400">The request is invalid.</response>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateHotel(Guid id, [FromBody] UpdateHotelCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in route does not match command ID.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing hotel from the system.
    /// </summary>
    /// <remarks>
    /// This endpoint permanently removes a hotel and all related data constraints must be respected.
    ///
    /// **Behavior:**
    /// - A hotel cannot be deleted if it is referenced by active or historical bookings (unless cascades are configured).
    /// - If the hotel does not exist, a <c>404 Not Found</c> is returned.
    ///
    /// This operation is intended for Admin use only.
    /// </remarks>
    /// <param name="id">The ID of the hotel to delete.</param>
    /// <response code="204">Hotel was successfully deleted.</response>
    /// <response code="404">No hotel was found with the given ID.</response>
    /// <response code="400">Invalid request.</response>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteHotel(Guid id)
    {
        await _mediator.Send(new DeleteHotelCommand(id));
        return NoContent();
    }
}
