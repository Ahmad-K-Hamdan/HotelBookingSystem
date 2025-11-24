using HotelBookingSystem.Application.Cities.Commands.CreateCity;
using HotelBookingSystem.Application.Cities.Queries.GetCities;
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
    /// <returns>A list of city DTOs.</returns>
    /// <response code="200">Successfully returned the list of cities.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<CityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCities()
        => Ok(await _mediator.Send(new GetCitiesQuery()));

    /// <summary>
    /// Creates a new city entry in the system.
    /// </summary>
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
        return Created(string.Empty, id);
    }
}
