using HotelBookingSystem.Application.Features.HotelRoomTypes.Commands.CreateHotelRoomType;
using HotelBookingSystem.Application.Features.HotelRoomTypes.Commands.DeleteHotelRoomType;
using HotelBookingSystem.Application.Features.HotelRoomTypes.Commands.UpdateHotelRoomType;
using HotelBookingSystem.Application.Features.HotelRoomTypes.Queries.GetHotelRoomTypeById;
using HotelBookingSystem.Application.Features.HotelRoomTypes.Queries.GetHotelRoomTypeById.Dtos;
using HotelBookingSystem.Application.Features.HotelRoomTypes.Queries.GetHotelRoomTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing hotel room types.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class HotelRoomTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the HotelRoomTypesController class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public HotelRoomTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// [Manager] Retrieves all hotel room types.
    /// </summary>
    /// <remarks>
    /// This endpoint is typically used in the admin or back-office UI to list
    /// all room types across all hotels.
    ///
    /// **Returned <c>HotelRoomTypeListDto</c> includes:**
    /// - <c>Id</c>, <c>HotelId</c>, <c>HotelName</c>.
    /// - <c>Name</c>, <c>PricePerNight</c>, <c>BedsCount</c>.
    /// - <c>MaxNumOfGuestsAdults</c>, <c>MaxNumOfGuestsChildren</c>.
    /// - <c>RoomsCount</c> – total rooms created for this type.
    /// </remarks>
    /// <response code="200">Successfully returned the list of room types.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<HotelRoomTypeListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetHotelRoomTypes()
        => Ok(await _mediator.Send(new GetHotelRoomTypesQuery()));

    /// <summary>
    /// [Manager] Retrieves a specific hotel room type by its ID,
    /// including its parent hotel and rooms.
    /// </summary>
    /// <remarks>
    /// Use this endpoint for detailed views of a room type, such as in an admin
    /// "Room Type Details" page.
    ///
    /// **Route parameter:**
    /// - <c>id</c> – the room type's unique identifier.
    ///
    /// **Returned <c>HotelRoomTypeDetailsDto</c> includes:**
    /// - Room type info: <c>Name</c>, <c>Description</c>, <c>PricePerNight</c>,
    ///   <c>BedsCount</c>, <c>MaxNumOfGuestsAdults</c>, <c>MaxNumOfGuestsChildren</c>.
    /// - <c>Hotel</c> – minimal hotel information:
    ///   - <c>Id</c>, <c>HotelName</c>, <c>CityName</c>, <c>CountryName</c>, <c>StarRating</c>.
    /// - <c>Rooms</c> – the rooms belonging to this type:
    ///   - <c>Id</c>, <c>RoomNumber</c>, <c>IsAvailable</c>, <c>CreatedAt</c>.
    /// </remarks>
    /// <param name="id">The ID of the room type to retrieve.</param>
    /// <response code="200">Successfully returned the room type.</response>
    /// <response code="404">No room type was found with the given ID.</response>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(HotelRoomTypeDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetHotelRoomTypeById(Guid id)
        => Ok(await _mediator.Send(new GetHotelRoomTypeByIdQuery(id)));

    /// <summary>
    /// [Manager] Creates a new room type for a hotel.
    /// </summary>
    /// <remarks>
    /// This endpoint is usually used by admins when configuring a hotel's inventory
    /// (e.g., "Standard Double Room", "Deluxe Suite").
    ///
    /// **Request body (<c>CreateHotelRoomTypeCommand</c>) includes:**
    /// - <c>HotelId</c> – the hotel this room type belongs to (must exist).
    /// - <c>Name</c> – room type name (e.g. “Deluxe King Room”).
    /// - <c>Description</c> – optional description (up to 1000 characters).
    /// - <c>PricePerNight</c> – base price per night.
    /// - <c>BedsCount</c> – number of beds.
    /// - <c>MaxNumOfGuestsAdults</c>, <c>MaxNumOfGuestsChildren</c>.
    /// </remarks>
    /// <param name="command">The command containing room type creation details.</param>
    /// <response code="200">Room type was successfully created and the new ID was returned.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateHotelRoomType([FromBody] CreateHotelRoomTypeCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetHotelRoomTypeById), new { id }, id);
    }

    /// <summary>
    /// [Manager] Updates an existing room type.
    /// </summary>
    /// <remarks>
    /// This endpoint updates the configuration of a room type.
    /// You can change its name, price, capacity, and even move it to another hotel
    /// by changing the <c>HotelId</c>.
    ///
    /// **Route parameter:**
    /// - <c>id</c> – the ID of the room type to update.
    ///
    /// **Request body (<c>UpdateHotelRoomTypeCommand</c>) includes:**
    /// - <c>Id</c> – must match the route <c>id</c>.
    /// - <c>HotelId</c> – target hotel (must exist).
    /// - <c>Name</c>, <c>Description</c>, <c>PricePerNight</c>.
    /// - <c>BedsCount</c>, <c>MaxNumOfGuestsAdults</c>, <c>MaxNumOfGuestsChildren</c>.
    /// </remarks>
    /// <param name="id">The ID of the room type to update.</param>
    /// <param name="command">The command containing updated room type data.</param>
    /// <response code="204">Room type was successfully updated.</response>
    /// <response code="404">No room type was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateHotelRoomType(Guid id, [FromBody] UpdateHotelRoomTypeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in route does not match command ID.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// [Manager] Deletes an existing room type.
    /// </summary>
    /// <remarks>
    /// **Route parameter:**
    /// - <c>id</c> – the ID of the room type to delete.
    /// </remarks>
    /// <param name="id">The ID of the room type to delete.</param>
    /// <response code="204">Room type was successfully deleted.</response>
    /// <response code="404">No room type was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteHotelRoomType(Guid id)
    {
        await _mediator.Send(new DeleteHotelRoomTypeCommand(id));
        return NoContent();
    }
}
