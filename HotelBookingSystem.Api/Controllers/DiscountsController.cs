using HotelBookingSystem.Application.Features.Discounts.Commands.CreateDiscount;
using HotelBookingSystem.Application.Features.Discounts.Commands.DeleteDiscount;
using HotelBookingSystem.Application.Features.Discounts.Commands.UpdateDiscount;
using HotelBookingSystem.Application.Features.Discounts.Queries.GetDiscounts;
using HotelBookingSystem.Application.Features.Discounts.Queries.GetDiscountById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing discount related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DiscountsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the DiscountsController class.
    /// </summary>   
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public DiscountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a list of all discounts available in the system.
    /// </summary>
    /// <returns>A list of discount DTOs.</returns>
    /// <response code="200">Successfully returned the list of discounts.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<DiscountDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDiscounts()
        => Ok(await _mediator.Send(new GetDiscountsQuery()));

    /// <summary>
    /// Creates a new discount entry in the system.
    /// </summary>
    /// <param name="command">The command containing discount creation details.</param>
    /// <returns>The ID of the newly created discount.</returns>
    /// <response code="201">Discount was successfully created.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetDiscountById), new { id }, id);
    }

    /// <summary>
    /// Retrieves the details of a specific discount by its ID.
    /// </summary>
    /// <param name="id">The ID of the discount to retrieve.</param>
    /// <returns>The details of the requested discount.</returns>
    /// <response code="200">Successfully returned the discount.</response>
    /// <response code="404">No discount was found with the given ID.</response>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DiscountDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDiscountById(Guid id)
        => Ok(await _mediator.Send(new GetDiscountByIdQuery(id)));

    /// <summary>
    /// Updates an existing discount entry in the system.
    /// </summary>
    /// <param name="id">The ID of the discount to update.</param>
    /// <param name="command">The command containing discount updated details.</param>
    /// <response code="204">Discount was successfully updated.</response>
    /// <response code="404">No discount was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDiscount(Guid id, [FromBody] UpdateDiscountCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in route does not match command ID.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing discount entry from the system.
    /// </summary>
    /// <param name="id">The ID of the discount to delete.</param>
    /// <response code="204">Discount was successfully deleted.</response>
    /// <response code="404">No discount was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteDiscount(Guid id)
    {
        await _mediator.Send(new DeleteDiscountCommand(id));
        return NoContent();
    }
}
