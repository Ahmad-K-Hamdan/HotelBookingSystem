using HotelBookingSystem.Application.Features.Amenities.Commands.CreateAmenity;
using HotelBookingSystem.Application.Features.Amenities.Commands.DeleteAmenity;
using HotelBookingSystem.Application.Features.Amenities.Commands.UpdateAmenity;
using HotelBookingSystem.Application.Features.Amenities.Queries.GetAmenities;
using HotelBookingSystem.Application.Features.Amenities.Queries.GetAmenityById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing amenity related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AmenitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the AmenitiesController class.
    /// </summary>   
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public AmenitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a list of all amenities available in the system.
    /// </summary>
    /// <returns>A list of amenity DTOs.</returns>
    /// <response code="200">Successfully returned the list of amenities.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<AmenityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAmenities()
        => Ok(await _mediator.Send(new GetAmenitiesQuery()));

    /// <summary>
    /// Creates a new amenity entry in the system.
    /// </summary>
    /// <param name="command">The command containing amenity creation details.</param>
    /// <returns>The ID of the newly created amenity.</returns>
    /// <response code="201">Amenity was successfully created.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAmenity([FromBody] CreateAmenityCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAmenityById), new { id }, id);
    }

    /// <summary>
    /// Retrieves the details of a specific amenity by its ID.
    /// </summary>
    /// <param name="id">The ID of the amenity to retrieve.</param>
    /// <returns>The details of the requested amenity.</returns>
    /// <response code="200">Successfully returned the amenity.</response>
    /// <response code="404">No amenity was found with the given ID.</response>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(AmenityDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAmenityById(Guid id)
        => Ok(await _mediator.Send(new GetAmenityByIdQuery(id)));

    /// <summary>
    /// Updates an existing amenity entry in the system.
    /// </summary>
    /// <param name="id">The ID of the amenity to update.</param>
    /// <param name="command">The command containing amenity updated details.</param>
    /// <response code="204">Amenity was successfully updated.</response>
    /// <response code="404">No amenity was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAmenity(Guid id, [FromBody] UpdateAmenityCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in route does not match command ID.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing amenity entry from the system.
    /// </summary>
    /// <param name="id">The ID of the amenity to delete.</param>
    /// <response code="204">Amenity was successfully deleted.</response>
    /// <response code="404">No amenity was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAmenity(Guid id)
    {
        await _mediator.Send(new DeleteAmenityCommand(id));
        return NoContent();
    }
}
