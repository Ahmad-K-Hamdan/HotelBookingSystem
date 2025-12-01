using HotelBookingSystem.Application.Features.PaymentMethods.Commands.CreatePaymentMethod;
using HotelBookingSystem.Application.Features.PaymentMethods.Commands.DeletePaymentMethod;
using HotelBookingSystem.Application.Features.PaymentMethods.Commands.UpdatePaymentMethod;
using HotelBookingSystem.Application.Features.PaymentMethods.Queries.GetPaymentMethodById;
using HotelBookingSystem.Application.Features.PaymentMethods.Queries.GetPaymentMethods;
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
    /// Retrieves all payment methods supported by the system.
    /// </summary>
    /// <remarks>
    /// This endpoint is used when building payment options during checkout or in
    /// admin configuration screens.
    ///
    /// A typical payment method might be "Credit Card", "Cash at Reception", etc.
    ///
    /// **Returned `PaymentMethodDto` (shape inferred):**
    /// - `Id` – payment method identifier.
    /// - `Name` (or similar) – display label shown to the user.
    /// </remarks>
    /// <returns>A list of payment methods.</returns>
    /// <response code="200">Successfully returned the list of payment methods.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<PaymentMethodDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentMethods()
        => Ok(await _mediator.Send(new GetPaymentMethodsQuery()));

    /// <summary>
    /// Retrieves the details of a specific payment method by its ID.
    /// </summary>
    /// <remarks>
    /// Typically used by admin tools to edit or review a specific payment method.
    /// </remarks>
    /// <param name="id">The ID of the payment method to retrieve.</param>
    /// <returns>Detailed information for the requested payment method.</returns>
    /// <response code="200">Successfully returned the payment method details.</response>
    /// <response code="404">No payment method was found with the given ID.</response>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(PaymentMethodDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentMethodById(Guid id)
        => Ok(await _mediator.Send(new GetPaymentMethodByIdQuery(id)));

    /// <summary>
    /// Creates a new payment method in the system.
    /// </summary>
    /// <remarks>
    /// Used by Admin to register a new way of paying.
    ///
    /// **Fields in <c>CreatePaymentMethodCommand</c>:**
    /// - `Name` – required, unique, used as a user-facing label.
    /// </remarks>
    /// <param name="command">The command containing payment method creation details.</param>
    /// <returns>The ID of the newly created payment method.</returns>
    /// <response code="201">Payment method was successfully created.</response>
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
    /// Updates an existing payment method.
    /// </summary>
    /// <remarks>
    /// Admins use this to rename or adjust properties of a payment method.
    ///
    /// The request body must contain a valid <c>UpdatePaymentMethodCommand</c>, which includes:
    /// - <c>Id</c> – must match the ID in the route.
    /// - Other editable properties of the payment method.
    /// </remarks>
    /// <param name="id">The ID of the payment method to update.</param>
    /// <param name="command">The updated payment method data.</param>
    /// <response code="204">Payment method was successfully updated.</response>
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
    /// Deletes a payment method from the system.
    /// </summary>
    /// <remarks>
    /// Intended for Admin use. 
    /// </remarks>
    /// <param name="id">The ID of the payment method to delete.</param>
    /// <response code="204">Payment method was successfully deleted.</response>
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
