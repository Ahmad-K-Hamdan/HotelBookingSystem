using HotelBookingSystem.Application.Features.Amenities.Commands.CreateAmenity;
using HotelBookingSystem.Application.Features.Amenities.Commands.DeleteAmenity;
using HotelBookingSystem.Application.Features.Amenities.Commands.UpdateAmenity;
using HotelBookingSystem.Application.Features.Amenities.Queries.GetAmenityById;
using HotelBookingSystem.Application.Features.Amenities.Queries.GetAmenities;
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
    /// <remarks>
    /// This endpoint is used to populate filters or chips in the UI where users can
    /// select required amenities (e.g., Wi-Fi, Parking, Pool).
    ///
    /// **Returned `AmenityDto` includes:**
    /// - `Id` – amenity identifier.
    /// - `Name` – display name
    /// - `Description` – optional longer text explaining the amenity.
    /// </remarks>
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
    /// <remarks>
    /// Used by Admin to register a new amenity that can be attached to hotels.
    ///
    /// **Fields in <c>CreateAmenityCommand</c>:**
    /// - `Name` – required, letters and spaces only, max length enforced.
    /// - `Description` – optional, with length and character restrictions.
    /// </remarks>
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
    /// <remarks>
    /// Returns the full amenity details, typically for admin edit forms.
    ///
    /// **Returned `AmenityDetailsDto` includes:**
    /// - `Id`, `Name`, `Description`.
    /// - `CreatedAt`, `UpdatedAt` timestamps.
    /// </remarks>
    /// <param name="id">The ID of the amenity to retrieve.</param>
    /// <returns>Detailed information for the requested amenity.</returns>
    /// <response code="200">Successfully returned the amenity details.</response>
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
    /// <remarks>
    /// Admins use this to modify the name or description of an amenity.
    ///
    /// The request body must contain a valid <c>UpdateAmenityCommand</c>, which includes:
    /// - <c>Id</c> – must match the ID in the route.
    /// - <c>Name</c>, <c>Description</c>.
    /// </remarks>
    /// <param name="id">The ID of the amenity to update.</param>
    /// <param name="command">The updated amenity data.</param>
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
    /// <remarks>
    /// Intended for Admin use only.
    /// </remarks>
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
