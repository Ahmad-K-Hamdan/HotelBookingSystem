using HotelBookingSystem.Application.Features.HotelGroups.Commands.CreateHotelGroup;
using HotelBookingSystem.Application.Features.HotelGroups.Commands.DeleteHotelGroup;
using HotelBookingSystem.Application.Features.HotelGroups.Commands.UpdateHotelGroup;
using HotelBookingSystem.Application.Features.HotelGroups.Queries.GetHotelGroups;
using HotelBookingSystem.Application.Features.HotelGroups.Queries.GetHotelGroupById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing hotel group related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HotelGroupsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the HotelGroupsController class.
    /// </summary>   
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public HotelGroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a list of all hotel groups available in the system.
    /// </summary>
    /// <returns>A list of hotel group DTOs.</returns>
    /// <response code="200">Successfully returned the list of hotel groups.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<HotelGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHotelGroups()
        => Ok(await _mediator.Send(new GetHotelGroupsQuery()));

    /// <summary>
    /// Creates a new hotel group entry in the system.
    /// </summary>
    /// <param name="command">The command containing hotel group creation details.</param>
    /// <returns>The ID of the newly created hotel group.</returns>
    /// <response code="201">HotelGroup was successfully created.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHotelGroup([FromBody] CreateHotelGroupCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetHotelGroupById), new { id }, id);
    }

    /// <summary>
    /// Retrieves the details of a specific hotel group by its ID.
    /// </summary>
    /// <param name="id">The ID of the hotel group to retrieve.</param>
    /// <returns>The details of the requested hotel group.</returns>
    /// <response code="200">Successfully returned the hotel group.</response>
    /// <response code="404">No hotel group was found with the given ID.</response>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(HotelGroupDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotelGroupById(Guid id)
        => Ok(await _mediator.Send(new GetHotelGroupByIdQuery(id)));

    /// <summary>
    /// Updates an existing hotel group entry in the system.
    /// </summary>
    /// <param name="id">The ID of the hotel group to update.</param>
    /// <param name="command">The command containing hotel group updated details.</param>
    /// <response code="204">HotelGroup was successfully updated.</response>
    /// <response code="404">No hotel group was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateHotelGroup(Guid id, [FromBody] UpdateHotelGroupCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in route does not match command ID.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing hotel group entry from the system.
    /// </summary>
    /// <param name="id">The ID of the hotel group to delete.</param>
    /// <response code="204">HotelGroup was successfully deleted.</response>
    /// <response code="404">No hotel group was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteHotelGroup(Guid id)
    {
        await _mediator.Send(new DeleteHotelGroupCommand(id));
        return NoContent();
    }
}
