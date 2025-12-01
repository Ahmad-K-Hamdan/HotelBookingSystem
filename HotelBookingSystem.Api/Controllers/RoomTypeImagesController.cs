using HotelBookingSystem.Application.Features.RoomTypeImages.Commands.CreateRoomTypeImage;
using HotelBookingSystem.Application.Features.RoomTypeImages.Commands.DeleteRoomTypeImage;
using HotelBookingSystem.Application.Features.RoomTypeImages.Commands.UpdateRoomTypeImage;
using HotelBookingSystem.Application.Features.RoomTypeImages.Queries.GetRoomTypeImageById;
using HotelBookingSystem.Application.Features.RoomTypeImages.Queries.GetRoomTypeImages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing room-type images.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RoomTypeImagesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the RoomTypeImagesController class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public RoomTypeImagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all images for all room types.
    /// </summary>
    /// <remarks>
    /// This endpoint is commonly used in the admin UI for managing room-type galleries.
    ///
    /// **Returned <c>RoomTypeImageListDto</c> includes:**
    /// - <c>Id</c>, <c>HotelRoomTypeId</c>, <c>RoomTypeName</c>
    /// - <c>HotelId</c>, <c>HotelName</c>
    /// - <c>Url</c>, <c>IsMain</c>
    /// </remarks>
    /// <response code="200">Successfully returned the list of room-type images.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<RoomTypeImageListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoomTypeImages()
        => Ok(await _mediator.Send(new GetRoomTypeImagesQuery()));

    /// <summary>
    /// Retrieves a specific room-type image by its ID.
    /// </summary>
    /// <remarks>
    /// This endpoint returns the image plus essential room-type and hotel info.
    ///
    /// **Route parameter:**
    /// - <c>id</c> – the image ID.
    ///
    /// **Returned <c>RoomTypeImageDetailsDto</c> includes:**
    /// - Image: <c>Id</c>, <c>Url</c>, <c>IsMain</c>
    /// - Room type: <c>HotelRoomTypeId</c>, <c>RoomTypeName</c>, <c>PricePerNight</c>,
    ///   <c>BedsCount</c>, <c>MaxNumOfGuestsAdults</c>, <c>MaxNumOfGuestsChildren</c>
    /// - Hotel: <c>HotelId</c>, <c>HotelName</c>
    /// </remarks>
    /// <param name="id">The ID of the room-type image to retrieve.</param>
    /// <response code="200">Successfully returned the room-type image.</response>
    /// <response code="404">No room-type image was found with the given ID.</response>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(RoomTypeImageDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoomTypeImageById(Guid id)
        => Ok(await _mediator.Send(new GetRoomTypeImageByIdQuery(id)));

    /// <summary>
    /// Creates a new image for a specific room type.
    /// </summary>
    /// <remarks>
    /// Use this when building the gallery for each room type (e.g. "Deluxe King").
    ///
    /// **Request body (<c>CreateRoomTypeImageCommand</c>) includes:**
    /// - <c>HotelRoomTypeId</c> – the parent room type.
    /// - <c>Url</c> – image URL.
    /// - <c>IsMain</c> – whether this is the main image for that room type.
    /// </remarks>
    /// <param name="command">The command containing room-type image data.</param>
    /// <response code="200">Room-type image was successfully created and the new ID was returned.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRoomTypeImage([FromBody] CreateRoomTypeImageCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
        // return CreatedAtAction(nameof(GetRoomTypeImageById), new { id }, id);
    }

    /// <summary>
    /// Updates an existing room-type image.
    /// </summary>
    /// <remarks>
    /// This can change which room type the image belongs to, update the URL, or toggle <c>IsMain</c>.
    ///
    /// **Route parameter:**
    /// - <c>id</c> – the ID of the image to update.
    ///
    /// **Request body (<c>UpdateRoomTypeImageCommand</c>) includes:**
    /// - <c>Id</c> – must match the route <c>id</c>.
    /// - <c>HotelRoomTypeId</c>, <c>Url</c>, <c>IsMain</c>.
    /// </remarks>
    /// <param name="id">The ID of the room-type image to update.</param>
    /// <param name="command">The command containing the updated image data.</param>
    /// <response code="204">Room-type image was successfully updated.</response>
    /// <response code="404">No room-type image was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRoomTypeImage(Guid id, [FromBody] UpdateRoomTypeImageCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in route does not match command ID.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing room-type image.
    /// </summary>
    /// <remarks>
    /// This permanently removes the image record.
    ///
    /// **Route parameter:**
    /// - <c>id</c> – the ID of the image to delete.
    /// </remarks>
    /// <param name="id">The ID of the room-type image to delete.</param>
    /// <response code="204">Room-type image was successfully deleted.</response>
    /// <response code="404">No room-type image was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteRoomTypeImage(Guid id)
    {
        await _mediator.Send(new DeleteRoomTypeImageCommand(id));
        return NoContent();
    }
}
