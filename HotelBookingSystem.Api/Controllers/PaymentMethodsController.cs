using HotelBookingSystem.Application.Features.PaymentMethods.Commands.CreatePaymentMethod;
using HotelBookingSystem.Application.Features.PaymentMethods.Commands.DeletePaymentMethod;
using HotelBookingSystem.Application.Features.PaymentMethods.Commands.UpdatePaymentMethod;
using HotelBookingSystem.Application.Features.PaymentMethods.Queries.GetPaymentMethods;
using HotelBookingSystem.Application.Features.PaymentMethods.Queries.GetPaymentMethodById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing payment method related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PaymentMethodsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the PaymentMethodsController class.
    /// </summary>   
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public PaymentMethodsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a list of all payment methods available in the system.
    /// </summary>
    /// <returns>A list of payment method DTOs.</returns>
    /// <response code="200">Successfully returned the list of payment methods.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<PaymentMethodDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentMethods()
        => Ok(await _mediator.Send(new GetPaymentMethodsQuery()));

    /// <summary>
    /// Creates a new payment method entry in the system.
    /// </summary>
    /// <param name="command">The command containing payment method creation details.</param>
    /// <returns>The ID of the newly created payment method.</returns>
    /// <response code="201">PaymentMethod was successfully created.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePaymentMethod([FromBody] CreatePaymentMethodCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetPaymentMethodById), new { id }, id);
    }

    /// <summary>
    /// Retrieves the details of a specific payment method by its ID.
    /// </summary>
    /// <param name="id">The ID of the payment method to retrieve.</param>
    /// <returns>The details of the requested payment method.</returns>
    /// <response code="200">Successfully returned the payment method.</response>
    /// <response code="404">No payment method was found with the given ID.</response>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(PaymentMethodDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentMethodById(Guid id)
        => Ok(await _mediator.Send(new GetPaymentMethodByIdQuery(id)));

    /// <summary>
    /// Updates an existing payment method entry in the system.
    /// </summary>
    /// <param name="id">The ID of the payment method to update.</param>
    /// <param name="command">The command containing payment method updated details.</param>
    /// <response code="204">PaymentMethod was successfully updated.</response>
    /// <response code="404">No payment method was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePaymentMethod(Guid id, [FromBody] UpdatePaymentMethodCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in route does not match command ID.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing payment method entry from the system.
    /// </summary>
    /// <param name="id">The ID of the payment method to delete.</param>
    /// <response code="204">PaymentMethod was successfully deleted.</response>
    /// <response code="404">No payment method was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePaymentMethod(Guid id)
    {
        await _mediator.Send(new DeletePaymentMethodCommand(id));
        return NoContent();
    }
}
