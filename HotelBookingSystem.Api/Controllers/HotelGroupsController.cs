using HotelBookingSystem.Application.Features.HotelGroups.Commands.CreateHotelGroup;
using HotelBookingSystem.Application.Features.HotelGroups.Commands.DeleteHotelGroup;
using HotelBookingSystem.Application.Features.HotelGroups.Commands.UpdateHotelGroup;
using HotelBookingSystem.Application.Features.HotelGroups.Queries.GetHotelGroupById;
using HotelBookingSystem.Application.Features.HotelGroups.Queries.GetHotelGroups;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing hotel group related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    /// Retrieves all hotel groups.
    /// </summary>
    /// <remarks>
    /// Hotel groups are parent organizations that own or manage multiple hotels
    /// (e.g., "Hilton", "Marriott").
    ///
    /// **Returned `HotelGroupDto` (shape inferred):**
    /// - `Id` – group identifier.
    /// - `GroupName` – display name of the group.
    /// - `Description` – optional description.
    /// </remarks>
    /// <returns>A list of hotel groups.</returns>
    /// <response code="200">Successfully returned the list of hotel groups.</response>
    [HttpGet]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<HotelGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHotelGroups()
        => Ok(await _mediator.Send(new GetHotelGroupsQuery()));

    /// <summary>
    /// Retrieves the details of a specific hotel group by its ID.
    /// </summary>
    /// <remarks>
    /// Returns basic information about a hotel group and is typically used by admin UIs.
    /// </remarks>
    /// <param name="id">The ID of the hotel group to retrieve.</param>
    /// <returns>Detailed information for the requested hotel group.</returns>
    /// <response code="200">Successfully returned the hotel group details.</response>
    /// <response code="404">No hotel group was found with the given ID.</response>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(HotelGroupDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotelGroupById(Guid id)
        => Ok(await _mediator.Send(new GetHotelGroupByIdQuery(id)));

    /// <summary>
    /// [Manager] Creates a new hotel group in the system.
    /// </summary>
    /// <remarks>
    /// Used by Admin to register a new hotel brand or chain.
    ///
    /// **Fields in <c>CreateHotelGroupCommand</c>:**
    /// - `GroupName` – required, unique name of the hotel group.
    /// - `Description` – optional additional information.
    /// </remarks>
    /// <param name="command">The command containing hotel group creation details.</param>
    /// <returns>The ID of the newly created hotel group.</returns>
    /// <response code="201">Hotel group was successfully created.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Authorize(Roles = "Manager")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateHotelGroup([FromBody] CreateHotelGroupCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetHotelGroupById), new { id }, id);
    }

    /// <summary>
    /// [Manager] Updates an existing hotel group.
    /// </summary>
    /// <remarks>
    /// Admins use this to rename or adjust details of a hotel group.
    ///
    /// The request body must contain a valid <c>UpdateHotelGroupCommand</c>, including:
    /// - <c>Id</c> – must match the ID in the route.
    /// </remarks>
    /// <param name="id">The ID of the hotel group to update.</param>
    /// <param name="command">The updated hotel group data.</param>
    /// <response code="204">Hotel group was successfully updated.</response>
    /// <response code="404">No hotel group was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Manager")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    /// [Manager] Deletes a hotel group from the system.
    /// </summary>
    /// <remarks>
    /// Intended for Admin use. Deletion may be constrained if hotels are still linked
    /// to the group.
    /// </remarks>
    /// <param name="id">The ID of the hotel group to delete.</param>
    /// <response code="204">Hotel group was successfully deleted.</response>
    /// <response code="404">No hotel group was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Manager")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteHotelGroup(Guid id)
    {
        await _mediator.Send(new DeleteHotelGroupCommand(id));
        return NoContent();
    }
}
