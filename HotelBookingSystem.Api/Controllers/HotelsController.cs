using HotelBookingSystem.Application.Features.Hotels.Commands.CreateHotel;
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
        return Ok(id);
        //return CreatedAtAction(nameof(GetHotelById), new { id }, id);
    }
}
