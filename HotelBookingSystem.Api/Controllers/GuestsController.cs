using HotelBookingSystem.Application.Features.Guests.Commands.CreateGuest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing guest related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GuestsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the GuestsController class.
    /// </summary>   
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public GuestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    ///// <summary>
    ///// Retrieves a list of all guests available in the system.
    ///// </summary>
    ///// <returns>A list of guest DTOs.</returns>
    ///// <response code="200">Successfully returned the list of guests.</response>
    //[HttpGet]
    //[Produces("application/json")]
    //[ProducesResponseType(typeof(IEnumerable<GuestDto>), StatusCodes.Status200OK)]
    //public async Task<IActionResult> GetGuests()
    //    => Ok(await _mediator.Send(new GetGuestsQuery()));

    /// <summary>
    /// Creates a new guest entry in the system.
    /// </summary>
    /// <param name="command">The command containing guest creation details.</param>
    /// <returns>The ID of the newly created guest.</returns>
    /// <response code="201">Guest was successfully created.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGuest([FromBody] CreateGuestCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
        //return CreatedAtAction(nameof(GetGuestById), new { id }, id);
    }

    ///// <summary>
    ///// Retrieves the details of a specific guest by its ID.
    ///// </summary>
    ///// <param name="id">The ID of the guest to retrieve.</param>
    ///// <returns>The details of the requested guest.</returns>
    ///// <response code="200">Successfully returned the guest.</response>
    ///// <response code="404">No guest was found with the given ID.</response>
    //[HttpGet("{id:guid}")]
    //[Produces("application/json")]
    //[ProducesResponseType(typeof(GuestDetailsDto), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> GetGuestById(Guid id)
    //    => Ok(await _mediator.Send(new GetGuestByIdQuery(id)));

    ///// <summary>
    ///// Updates an existing guest entry in the system.
    ///// </summary>
    ///// <param name="id">The ID of the guest to update.</param>
    ///// <param name="command">The command containing guest updated details.</param>
    ///// <response code="204">Guest was successfully updated.</response>
    ///// <response code="404">No guest was found with the given ID.</response>
    ///// <response code="400">The request was invalid.</response>
    //[HttpPut("{id:guid}")]
    //[Produces("application/json")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> UpdateGuest(Guid id, [FromBody] UpdateGuestCommand command)
    //{
    //    if (id != command.Id)
    //    {
    //        return BadRequest("ID in route does not match command ID.");
    //    }

    //    await _mediator.Send(command);
    //    return NoContent();
    //}

    ///// <summary>
    ///// Deletes an existing guest entry from the system.
    ///// </summary>
    ///// <param name="id">The ID of the guest to delete.</param>
    ///// <response code="204">Guest was successfully deleted.</response>
    ///// <response code="404">No guest was found with the given ID.</response>
    ///// <response code="400">The request was invalid.</response>
    //[HttpDelete("{id:guid}")]
    //[Produces("application/json")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> DeleteGuest(Guid id)
    //{
    //    await _mediator.Send(new DeleteGuestCommand(id));
    //    return NoContent();
    //}
}
