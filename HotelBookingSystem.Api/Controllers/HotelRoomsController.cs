using HotelBookingSystem.Application.Features.HotelRooms.Commands.CreateHotelRoom;
using HotelBookingSystem.Application.Features.HotelRooms.Commands.DeleteHotelRoom;
using HotelBookingSystem.Application.Features.HotelRooms.Commands.UpdateHotelRoom;
using HotelBookingSystem.Application.Features.HotelRooms.Queries.GetHotelRoomById;
using HotelBookingSystem.Application.Features.HotelRooms.Queries.GetHotelRoomById.Dtos;
using HotelBookingSystem.Application.Features.HotelRooms.Queries.GetHotelRooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing hotel rooms.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HotelRoomsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the HotelRoomsController class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public HotelRoomsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all hotel rooms.
    /// </summary>
    /// <remarks>
    /// This endpoint is typically used from an admin/back-office interface to list
    /// all rooms across all hotels and room types.
    ///
    /// **Returned <c>HotelRoomListDto</c> includes:**
    /// - Room info: <c>Id</c>, <c>HotelRoomTypeId</c>, <c>RoomNumber</c>, <c>IsAvailable</c>.
    /// - Room type and hotel info: <c>HotelId</c>, <c>HotelName</c>, <c>RoomTypeName</c>.
    /// </remarks>
    /// <response code="200">Successfully returned the list of rooms.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<HotelRoomListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHotelRooms()
        => Ok(await _mediator.Send(new GetHotelRoomsQuery()));

    /// <summary>
    /// Retrieves a specific hotel room by its ID,
    /// including its room type details and images.
    /// </summary>
    /// <remarks>
    /// This endpoint is designed for an admin "Room Details" view.
    ///
    /// **Route parameter:**
    /// - <c>id</c> – the room's unique identifier.
    ///
    /// **Returned <c>HotelRoomDetailsDto</c> includes:**
    /// - Room info: <c>Id</c>, <c>HotelRoomTypeId</c>, <c>RoomNumber</c>, <c>IsAvailable</c>,
    ///   <c>CreatedAt</c>, <c>UpdatedAt</c>.
    /// - <c>RoomType</c> – room type details:
    ///   - <c>Id</c>, <c>HotelId</c>, <c>RoomTypeName</c>, <c>Description</c>,
    ///     <c>PricePerNight</c>, <c>BedsCount</c>,
    ///     <c>MaxNumOfGuestsAdults</c>, <c>MaxNumOfGuestsChildren</c>.
    /// - <c>Images</c> – all images for this room:
    ///   - <c>Id</c>, <c>Url</c>, <c>IsMain</c>.
    /// </remarks>
    /// <param name="id">The ID of the room to retrieve.</param>
    /// <response code="200">Successfully returned the room details.</response>
    /// <response code="404">No room was found with the given ID.</response>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(HotelRoomDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotelRoomById(Guid id)
        => Ok(await _mediator.Send(new GetHotelRoomByIdQuery(id)));

    /// <summary>
    /// Creates a new room under a specific room type.
    /// </summary>
    /// <remarks>
    /// This endpoint is usually used when configuring a hotel's inventory
    /// by adding concrete rooms (e.g. room 101, 102, etc.).
    ///
    /// **Request body (<c>CreateHotelRoomCommand</c>) includes:**
    /// - <c>HotelRoomTypeId</c> – the room type this room belongs to (must exist).
    /// - <c>RoomNumber</c> – numeric room identifier (e.g. 101).
    /// - <c>IsAvailable</c> – initial availability flag.
    /// </remarks>
    /// <param name="command">The command containing room creation details.</param>
    /// <response code="200">Room was successfully created and the new ID was returned.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHotelRoom([FromBody] CreateHotelRoomCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetHotelRoomById), new { id }, id);
    }

    /// <summary>
    /// Updates an existing room.
    /// </summary>
    /// <remarks>
    /// This endpoint can be used to change the room number, availability, or move the room
    /// to another room type (by changing <c>HotelRoomTypeId</c>).
    ///
    /// **Route parameter:**
    /// - <c>id</c> – the ID of the room to update.
    ///
    /// **Request body (<c>UpdateHotelRoomCommand</c>) includes:**
    /// - <c>Id</c> – must match the route <c>id</c>.
    /// - <c>HotelRoomTypeId</c> – target room type (must exist).
    /// - <c>RoomNumber</c>, <c>IsAvailable</c>.
    /// </remarks>
    /// <param name="id">The ID of the room to update.</param>
    /// <param name="command">The command containing updated room data.</param>
    /// <response code="204">Room was successfully updated.</response>
    /// <response code="404">No room was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateHotelRoom(Guid id, [FromBody] UpdateHotelRoomCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in route does not match command ID.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing room.
    /// </summary>
    /// <remarks>
    /// **Route parameter:**
    /// - <c>id</c> – the ID of the room to delete.
    /// </remarks>
    /// <param name="id">The ID of the room to delete.</param>
    /// <response code="204">Room was successfully deleted.</response>
    /// <response code="404">No room was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteHotelRoom(Guid id)
    {
        await _mediator.Send(new DeleteHotelRoomCommand(id));
        return NoContent();
    }
}
