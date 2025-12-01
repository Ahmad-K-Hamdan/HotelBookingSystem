using HotelBookingSystem.Application.Features.Cities.Commands.CreateCity;
using HotelBookingSystem.Application.Features.Cities.Commands.DeleteCity;
using HotelBookingSystem.Application.Features.Cities.Commands.UpdateCity;
using HotelBookingSystem.Application.Features.Cities.Queries.GetCities;
using HotelBookingSystem.Application.Features.Cities.Queries.GetCityById;
using HotelBookingSystem.Application.Features.Cities.Queries.GetTrendingCities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing city related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the CitiesController class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public CitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a list of all cities available in the system.
    /// </summary>
    /// <remarks>
    /// This endpoint is typically used to populate dropdowns or filters where the user
    /// can choose a destination city.
    ///
    /// **Returned `CityDto` includes:**
    /// - `Id` – unique identifier for the city.
    /// - `CityName` – name of the city.
    /// - `CountryName` – country in which the city is located.
    /// - `Description` – optional description or marketing text for the city.
    /// </remarks>
    /// <returns>A list of city DTOs.</returns>
    /// <response code="200">Successfully returned the list of cities.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<CityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCities()
        => Ok(await _mediator.Send(new GetCitiesQuery()));

    /// <summary>
    /// Retrieves the details of a specific city by its ID.
    /// </summary>
    /// <remarks>
    /// This endpoint returns full information about a single city and is useful for
    /// admin screens or detail views.
    ///
    /// **Returned `CityDetailsDto` includes:**
    /// - `Id`, `CityName`, `CountryName`, `Description`.
    /// - `CreatedAt` – time the city entry was created.
    /// - `UpdatedAt` – last update time (if any).
    /// </remarks>
    /// <param name="id">The ID of the city to retrieve.</param>
    /// <returns>Detailed information for the requested city.</returns>
    /// <response code="200">Successfully returned the city details.</response>
    /// <response code="404">No city was found with the given ID.</response>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CityDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCityById(Guid id)
        => Ok(await _mediator.Send(new GetCityByIdQuery(id)));

    /// <summary>
    /// Creates a new city entry in the system.
    /// </summary>
    /// <remarks>
    /// This endpoint is used by the Admin interface to register a new city that can be
    /// associated with hotels.
    ///
    /// **Required fields in <c>CreateCityCommand</c>:**
    /// - `CityName` – must be non-empty, letters and spaces only, max length enforced.
    /// - `CountryName` – must be non-empty, letters and spaces only, max length enforced.
    /// - `Description` – optional, with maximum length and character restrictions.
    /// </remarks>
    /// <param name="command">The command containing city creation details.</param>
    /// <returns>The ID of the newly created city.</returns>
    /// <response code="201">City was successfully created.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCity([FromBody] CreateCityCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCityById), new { id }, id);
    }

    /// <summary>
    /// Updates an existing city entry in the system.
    /// </summary>
    /// <remarks>
    /// This endpoint is used by the Admin interface to update a city's name, country,
    /// or description.
    ///
    /// The request body must contain a valid <c>UpdateCityCommand</c>, which includes:
    /// - <c>Id</c> – must match the city ID in the route.
    /// - <c>CityName</c>, <c>CountryName</c>, <c>Description</c>.
    /// </remarks>
    /// <param name="id">The ID of the city to update.</param>
    /// <param name="command">The updated city data.</param>
    /// <response code="204">City was successfully updated.</response>
    /// <response code="404">No city was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCity(Guid id, [FromBody] UpdateCityCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in route does not match command ID.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing city entry from the system.
    /// </summary>
    /// <remarks>
    /// This operation is intended for Admin use only.
    /// </remarks>
    /// <param name="id">The ID of the city to delete.</param>
    /// <response code="204">City was successfully deleted.</response>
    /// <response code="404">No city was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCity(Guid id)
    {
        await _mediator.Send(new DeleteCityCommand(id));
        return NoContent();
    }

    /// <summary>
    /// Retrieves trending cities based on visit counts.
    /// </summary>
    /// <remarks>
    /// This endpoint returns the cities that have been visited the most in the system,
    /// based on aggregated hotel visit logs.
    ///
    /// **Query parameters:**
    /// - `Limit` – optional, default is 5. Maximum number of cities to return (max 20).
    /// - `DaysBack` – optional. If provided (e.g., 30 or 90), only visits within the last N days are counted.
    ///   If omitted, all-time visits are considered.
    ///
    /// **Returned `TrendingCityDto` includes:**
    /// - `CityId`, `CityName`, `CountryName`.
    /// - `VisitCount` – number of visits aggregated across all hotels in that city.
    ///
    /// This is used for the "Trending Destination Highlights" section on the home page.
    /// </remarks>
    /// <response code="200">Successfully returned the list of trending cities.</response>
    [HttpGet("trending")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<TrendingCityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrendingCities([FromQuery] GetTrendingCitiesQuery query)
        => Ok(await _mediator.Send(query));
}
