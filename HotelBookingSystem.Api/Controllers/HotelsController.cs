using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelDetailsById;
using HotelBookingSystem.Application.Features.Hotels.Commands.CreateHotel;
using HotelBookingSystem.Application.Features.Hotels.Commands.DeleteHotel;
using HotelBookingSystem.Application.Features.Hotels.Commands.UpdateHotel;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelDetailsById.Dtos;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels.Dtos;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetRecentlyVisited;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing hotel related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    /// This endpoint is intended for the main Search Results page.
    ///
    /// It determines whether each hotel can fully accommodate the user’s request
    /// (dates + number of rooms + adults/children), and for every eligible hotel:
    ///
    /// **It returns only the *best matching* combination of rooms** — the cheapest valid
    /// set of rooms that satisfies the user’s request.
    ///
    /// This combination is exposed in `MatchedRooms`, and the pricing fields:
    /// - `MinTotalOriginalPricePerNight`  
    /// - `MinTotalDiscountedPricePerNight`  
    /// represent the **total nightly cost for the entire matched combination.**
    ///
    /// ---
    ///
    /// **Important query parameters:**
    /// - `CheckInDate`, `CheckOutDate` – required; default today/tomorrow.
    /// - `Rooms` – one or more room requests (adults/children).
    /// - `CityName`, `CountryName` – location filters.
    /// - `MinStars` – minimum star rating.
    /// - `MinPrice`, `MaxPrice` – filter by discounted total nightly cost.
    /// - `OnlyWithActiveDiscount` – active discount only.
    /// - `AmenityIds`, `RoomTypes` – amenity or room-type filters.
    /// - `Sort` – price, stars, popularity, etc.
    /// - `Limit` – limit number of hotels returned.
    ///
    /// **Returned `HotelDto` includes:**
    /// - Main hotel data (name, city, star rating, group).
    /// - Active discount indicator and visit count.
    /// - Pricing for the matched room combination.
    /// - `MatchedRooms`: the rooms chosen as the best match.
    /// - Main hotel image and amenity names.
    /// </remarks>
    [HttpGet]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<HotelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetHotels([FromQuery] GetHotelsQuery getHotelsQuery)
        => Ok(await _mediator.Send(getHotelsQuery));

    /// <summary>
    /// [Manager] Creates a new hotel in the system.
    /// </summary>
    /// <remarks>
    /// This endpoint is used by the Admin interface to register a new hotel.
    ///
    /// **Required fields in `CreateHotelDto`:**
    /// - `HotelGroupId` — must reference an existing hotel group.
    /// - `CityId` — the ID of the city where the hotel is located.
    /// - `HotelName` and `HotelAddress` — the hotel's identity and physical location.
    /// - `StarRating`, `Latitude`, `Longitude`, and `Description` — essential rating and geo information.
    /// </remarks>
    /// <param name="createHotelDto">The data required to create a new hotel.</param>
    /// <returns>The ID of the newly created hotel.</returns>
    /// <response code="200">Hotel was successfully created and the new ID was returned.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Authorize(Roles = "Manager")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDto createHotelDto)
    {
        var id = await _mediator.Send(new CreateHotelCommand(createHotelDto));
        return CreatedAtAction(nameof(GetHotelDetailsById), new { id }, id);
    }

    /// <summary>
    /// Retrieves detailed information about a specific hotel.
    /// </summary>
    /// <remarks>
    /// This endpoint is intended for the **Hotel Details Page**.
    ///
    /// Unlike the search endpoint, which returns only the *best matching* room combination,
    /// this endpoint returns **all room types** that are available for the specified date range.
    ///
    /// The result includes:
    /// - Full hotel info (name, address, rating, group, description).
    /// - All hotel images.
    /// - Amenity names.
    /// - **All available room types** with:
    ///     - Capacity
    ///     - Original + discounted pricing
    ///     - Availability based on the date range
    ///     - Room-type images
    /// - Reviews, average rating, and review count.
    ///
    /// **Date range:**
    /// - `checkInDate` / `checkOutDate` control room-type availability.
    /// - Defaults: today → tomorrow.
    ///
    /// **Availability logic:**
    /// - A room type is considered available if **any room** in that type is free across the date range.
    /// - Individual room numbers are not returned here.
    /// </remarks>
    /// <param name="id">The ID of the hotel to retrieve.</param>
    /// <param name="checkInDate">Optional check-in date (defaults to today).</param>
    /// <param name="checkOutDate">Optional check-out date (defaults to tomorrow).</param>
    /// <returns>Detailed information for the requested hotel.</returns>
    /// <response code="200">Successfully returned the hotel details.</response>
    /// <response code="404">No hotel was found with the given ID.</response>
    /// <response code="400">Invalid request, such as an invalid date range.</response>
    [HttpGet("{id:guid}/details")]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(HotelDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetHotelDetailsById(Guid id, [FromQuery] DateOnly? checkInDate, [FromQuery] DateOnly? checkOutDate)
    {
        return Ok(await _mediator.Send(new GetHotelDetailsByIdQuery(id, checkInDate, checkOutDate)));
    }

    /// <summary>
    /// [Manager] Updates an existing hotel in the system.
    /// </summary>
    /// <remarks>
    /// This endpoint is used by the Admin interface to modify hotel information.
    ///
    /// The request body must contain a valid <c>UpdateHotelCommand</c>, which includes:
    /// - <c>Id</c> — must match the hotel ID in the route.
    /// - <c>HotelName</c>, <c>HotelAddress</c>, <c>StarRating</c>, <c>Latitude</c>, <c>Longitude</c>, etc.
    /// - <c>HotelGroupId</c> and <c>CityId</c> must reference existing entities.
    /// </remarks>
    /// <param name="id">The ID of the hotel to update.</param>
    /// <param name="command">The updated hotel information.</param>
    /// <response code="204">Hotel was successfully updated.</response>
    /// <response code="404">No hotel was found with the given ID.</response>
    /// <response code="400">The request is invalid.</response>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Manager")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    /// [Manager] Deletes an existing hotel from the system.
    /// </summary>
    /// <remarks>
    /// This operation is intended for Admin use only.
    /// </remarks>
    /// <param name="id">The ID of the hotel to delete.</param>
    /// <response code="204">Hotel was successfully deleted.</response>
    /// <response code="404">No hotel was found with the given ID.</response>
    /// <response code="400">Invalid request.</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Manager")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteHotel(Guid id)
    {
        await _mediator.Send(new DeleteHotelCommand(id));
        return NoContent();
    }

    /// <summary>
    /// [Authenticated] Retrieves the current user's recently visited hotels.
    /// </summary>
    /// <remarks>
    /// This endpoint returns a personalized list of hotels recently viewed by the authenticated user,
    /// based on the visit logs recorded when the user opens a hotel details page.
    ///
    /// **Behavior:**
    /// - Uses the user ID from the JWT to identify the current user.
    /// - Groups visit logs by hotel and returns each hotel only once (most recent visit wins).
    /// - Ordered by latest visit time (most recently visited first).
    ///
    /// **Query parameters:**
    /// - `Limit` – optional, default is 5. Controls how many hotels are returned (max 20).
    ///
    /// **Returned `RecentHotelDto` includes:**
    /// - `HotelId`, `HotelName`, `CityName`, `CountryName`, `StarRating`.
    /// - `MainImageUrl` if available.
    /// - `MinOriginalPricePerNight` and `MinDiscountedPricePerNight` calculated from the hotel's room types
    ///   and active discount (if any).
    /// - `LastVisitedAt` – timestamp of the most recent visit by this user.
    /// </remarks>
    /// <response code="200">Successfully returned the list of recently visited hotels.</response>
    /// <response code="401">User is not authenticated.</response>
    [HttpGet("recently-visited")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<RecentHotelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRecentlyVisitedHotels([FromQuery] GetRecentlyVisitedHotelsQuery query)
        => Ok(await _mediator.Send(query));
}
