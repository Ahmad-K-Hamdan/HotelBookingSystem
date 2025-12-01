using HotelBookingSystem.Application.Features.HotelImages.Commands.CreateHotelImage;
using HotelBookingSystem.Application.Features.HotelImages.Commands.DeleteHotelImage;
using HotelBookingSystem.Application.Features.HotelImages.Commands.UpdateHotelImage;
using HotelBookingSystem.Application.Features.HotelImages.Queries.GetHotelImageById;
using HotelBookingSystem.Application.Features.HotelImages.Queries.GetHotelImages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing hotel-level images.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HotelImagesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the HotelImagesController class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public HotelImagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all images for all hotels.
    /// </summary>
    /// <remarks>
    /// This endpoint is typically used from an admin interface to manage
    /// hotel hero / gallery images.
    ///
    /// **Returned <c>HotelImageListDto</c> includes:**
    /// - <c>Id</c>, <c>HotelId</c>, <c>HotelName</c>
    /// - <c>Url</c>, <c>IsMain</c>
    /// </remarks>
    /// <response code="200">Successfully returned the list of hotel images.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<HotelImageListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHotelImages()
        => Ok(await _mediator.Send(new GetHotelImagesQuery()));

    /// <summary>
    /// Retrieves a specific hotel image by its ID.
    /// </summary>
    /// <remarks>
    /// This endpoint returns both the image information and basic hotel details.
    ///
    /// **Route parameter:**
    /// - <c>id</c> – the image ID.
    ///
    /// **Returned <c>HotelImageDetailsDto</c> includes:**
    /// - Image: <c>Id</c>, <c>Url</c>, <c>IsMain</c>
    /// - Hotel: <c>HotelId</c>, <c>HotelName</c>, <c>HotelAddress</c>
    /// </remarks>
    /// <param name="id">The ID of the hotel image to retrieve.</param>
    /// <response code="200">Successfully returned the hotel image.</response>
    /// <response code="404">No hotel image was found with the given ID.</response>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(HotelImageDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotelImageById(Guid id)
        => Ok(await _mediator.Send(new GetHotelImageByIdQuery(id)));

    /// <summary>
    /// Creates a new image for a hotel.
    /// </summary>
    /// <remarks>
    /// This endpoint is usually used from an admin CMS to attach images to a hotel.
    ///
    /// **Request body (<c>CreateHotelImageCommand</c>) includes:**
    /// - <c>HotelId</c> – the parent hotel.
    /// - <c>Url</c> – image URL (e.g. CDN path).
    /// - <c>IsMain</c> – whether this image is the main/cover image.
    /// </remarks>
    /// <param name="command">The command containing hotel image data.</param>
    /// <response code="200">Hotel image was successfully created and the new ID was returned.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHotelImage([FromBody] CreateHotelImageCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
        // return CreatedAtAction(nameof(GetHotelImageById), new { id }, id);
    }

    /// <summary>
    /// Updates an existing hotel image.
    /// </summary>
    /// <remarks>
    /// You can use this endpoint to change the image URL, toggle <c>IsMain</c>,
    /// or move the image to a different hotel.
    ///
    /// **Route parameter:**
    /// - <c>id</c> – the ID of the image to update.
    ///
    /// **Request body (<c>UpdateHotelImageCommand</c>) includes:**
    /// - <c>Id</c> – must match the route <c>id</c>.
    /// - <c>HotelId</c>, <c>Url</c>, <c>IsMain</c>.
    /// </remarks>
    /// <param name="id">The ID of the image to update.</param>
    /// <param name="command">The command containing updated image data.</param>
    /// <response code="204">Hotel image was successfully updated.</response>
    /// <response code="404">No hotel image was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateHotelImage(Guid id, [FromBody] UpdateHotelImageCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in route does not match command ID.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing hotel image.
    /// </summary>
    /// <remarks>
    /// This operation permanently removes the image record from the system.
    ///
    /// **Route parameter:**
    /// - <c>id</c> – the ID of the image to delete.
    /// </remarks>
    /// <param name="id">The ID of the image to delete.</param>
    /// <response code="204">Hotel image was successfully deleted.</response>
    /// <response code="404">No hotel image was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteHotelImage(Guid id)
    {
        await _mediator.Send(new DeleteHotelImageCommand(id));
        return NoContent();
    }
}
