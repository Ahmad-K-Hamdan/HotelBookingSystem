using HotelBookingSystem.Application.Features.Discounts.Commands.CreateDiscount;
using HotelBookingSystem.Application.Features.Discounts.Commands.DeleteDiscount;
using HotelBookingSystem.Application.Features.Discounts.Commands.UpdateDiscount;
using HotelBookingSystem.Application.Features.Discounts.Queries.GetDiscountById;
using HotelBookingSystem.Application.Features.Discounts.Queries.GetDiscounts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing discount related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
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
    /// [Manager] Retrieves a list of all discounts available in the system.
    /// </summary>
    /// <remarks>
    /// This endpoint is typically used by admin tools or back-office UIs to view all
    /// configured discounts.
    ///
    /// **Returned `DiscountDto` includes:**
    /// - `Id` – unique discount identifier.
    /// - `DiscountDescription` – human readable description (e.g., "Eid Offer 2025").
    /// - `DiscountRate` – decimal between 0.01 and 1.00 (1%–100%).
    /// - `IsActive` – whether this discount is currently active.
    /// </remarks>
    /// <returns>A list of discount DTOs.</returns>
    /// <response code="200">Successfully returned the list of discounts.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<DiscountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetDiscounts()
        => Ok(await _mediator.Send(new GetDiscountsQuery()));

    /// <summary>
    /// [Manager] Creates a new discount entry in the system.
    /// </summary>
    /// <remarks>
    /// Used by the Admin interface to define new promotional discounts that can be
    /// attached to hotels.
    ///
    /// **Fields in <c>CreateDiscountCommand</c>:**
    /// - `DiscountDescription` – required, max length, restricted characters.
    /// - `DiscountRate` – required, between 0.01 and 1.00.
    /// - `IsActive` – whether the discount should start active.
    ///
    /// If an identical discount (same description and rate) already exists,
    /// a duplicate-record error is thrown and mapped to an error response.
    /// </remarks>
    /// <param name="command">The command containing discount creation details.</param>
    /// <returns>The ID of the newly created discount.</returns>
    /// <response code="201">Discount was successfully created.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetDiscountById), new { id }, id);
    }

    /// <summary>
    /// [Manager] Retrieves the details of a specific discount by its ID.
    /// </summary>
    /// <remarks>
    /// Returns the full discount information as stored in the system.
    ///
    /// **Returned `DiscountDetailsDto` includes:**
    /// - `Id`, `DiscountDescription`, `DiscountRate`, `IsActive`.
    /// </remarks>
    /// <param name="id">The ID of the discount to retrieve.</param>
    /// <returns>Detailed information for the requested discount.</returns>
    /// <response code="200">Successfully returned the discount details.</response>
    /// <response code="404">No discount was found with the given ID.</response>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DiscountDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetDiscountById(Guid id)
        => Ok(await _mediator.Send(new GetDiscountByIdQuery(id)));

    /// <summary>
    /// [Manager] Updates an existing discount entry in the system.
    /// </summary>
    /// <remarks>
    /// Admins can use this endpoint to adjust a discount's description, rate, or active status.
    ///
    /// The request body must contain a valid <c>UpdateDiscountCommand</c>, including:
    /// - <c>Id</c> – must match the ID in the route.
    /// - <c>DiscountDescription</c>, <c>DiscountRate</c>, <c>IsActive</c>.
    ///
    /// **Validation:**
    /// - Route <c>id</c> must match <c>command.Id</c> or the request is rejected.
    /// - FluentValidation enforces constraints on all fields.
    /// - If the discount does not exist, a <c>404 Not Found</c> is returned.
    /// </remarks>
    /// <param name="id">The ID of the discount to update.</param>
    /// <param name="command">The updated discount data.</param>
    /// <response code="204">Discount was successfully updated.</response>
    /// <response code="404">No discount was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    /// [Manager] Deletes an existing discount entry from the system.
    /// </summary>
    /// <remarks>
    /// Intended for Admin use only.
    ///
    /// If the discount does not exist, a <c>404 Not Found</c> is returned.
    /// The ability to delete may be constrained by how existing bookings reference discounts.
    /// </remarks>
    /// <param name="id">The ID of the discount to delete.</param>
    /// <response code="204">Discount was successfully deleted.</response>
    /// <response code="404">No discount was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteDiscount(Guid id)
    {
        await _mediator.Send(new DeleteDiscountCommand(id));
        return NoContent();
    }
}
