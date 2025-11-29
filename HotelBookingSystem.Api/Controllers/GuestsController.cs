using HotelBookingSystem.Application.Features.Guests.Commands.CreateGuest;
using HotelBookingSystem.Application.Features.Guests.Commands.DeleteGuest;
using HotelBookingSystem.Application.Features.Guests.Commands.UpdateGuest;
using HotelBookingSystem.Application.Features.Guests.Queries.GetGuestById;
using HotelBookingSystem.Application.Features.Guests.Queries.GetGuestById.Dtos;
using HotelBookingSystem.Application.Features.Guests.Queries.GetGuests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing guest related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GuestsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuestsController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public GuestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all guests in the system.
    /// </summary>
    /// <remarks>
    /// This endpoint is typically intended for administrative views,
    /// where a list of all registered guests is required.
    ///
    /// **Returned <c>GuestListDto</c> includes:**
    /// - <c>Id</c> – internal guest identifier.
    /// - <c>UserId</c> – the underlying identity user ID.
    /// - <c>PassportNumber</c> – the guest's passport number (as stored).
    /// - <c>HomeCountry</c> – the guest's home country.
    /// </remarks>
    /// <response code="200">Successfully returned the list of guests.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<GuestListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGuests()
        => Ok(await _mediator.Send(new GetGuestsQuery()));

    /// <summary>
    /// Retrieves detailed information about a specific guest.
    /// </summary>
    /// <remarks>
    /// This endpoint returns the guest's basic profile, along with
    /// all of their bookings and reviews.
    ///
    /// **Route parameter:**
    /// - <c>id</c> – the guest's unique identifier.
    ///
    /// **Returned <c>GuestDetailsDto</c> includes:**
    /// - Guest info: <c>Id</c>, <c>UserId</c>, <c>PassportNumber</c>, <c>HomeCountry</c>.
    /// - <c>Reviews</c> – a list of <c>GuestReviewDto</c>:
    ///   - <c>ReviewId</c>, <c>HotelId</c>, <c>HotelName</c>, <c>Rating</c>, <c>Comment</c>, <c>ReviewDate</c>.
    /// - <c>Bookings</c> – a list of <c>GuestBookingDto</c>:
    ///   - <c>BookingId</c>, <c>HotelId</c>, <c>HotelName</c>,
    ///     <c>CheckInDate</c>, <c>CheckOutDate</c>,
    ///     <c>NumOfAdults</c>, <c>NumOfChildren</c>,
    ///     <c>SpecialRequests</c>, <c>ConfirmationCode</c>, <c>CreatedAt</c>.
    /// </remarks>
    /// <param name="id">The ID of the guest to retrieve.</param>
    /// <response code="200">Successfully returned the guest details.</response>
    /// <response code="404">No guest was found with the given ID.</response>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GuestDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGuestById(Guid id)
        => Ok(await _mediator.Send(new GetGuestByIdQuery(id)));

    /// <summary>
    /// Creates a new guest profile for the currently authenticated user.
    /// </summary>
    /// <remarks>
    /// This endpoint is typically called after a user account is created
    /// and you want to store additional guest-specific information such as
    /// passport number and home country.
    ///
    /// The underlying handler uses the current authenticated user from the JWT
    /// (via <c>ICurrentUserService</c>), so the <c>UserId</c> is not sent in the request body.
    ///
    /// **Request body (<c>CreateGuestCommand</c>) includes:**
    /// - <c>PassportNumber</c> – required, validated by FluentValidation.
    /// - <c>HomeCountry</c> – required, validated by FluentValidation.
    ///
    /// On success, the API returns the Guid identifier of the newly created guest.
    /// </remarks>
    /// <param name="command">The command containing guest creation details.</param>
    /// <response code="200">Guest was successfully created and the new ID was returned.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGuest([FromBody] CreateGuestCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetGuestById), new { id }, id);
    }

    /// <summary>
    /// Updates an existing guest’s profile.
    /// </summary>
    /// <remarks>
    /// This endpoint allows updating a guest’s **passport number** and **home country**.
    /// The guest’s underlying <c>UserId</c> (Identity User) **cannot** be changed.
    ///
    /// **Route parameter:**
    /// - <c>id</c> — the unique identifier of the guest to update.
    ///
    /// **Request body (<c>UpdateGuestCommand</c>) includes:**
    /// - <c>PassportNumber</c> — required, validated.
    /// - <c>HomeCountry</c> — required, validated.
    ///
    /// On success, the endpoint returns **204 No Content**.
    /// </remarks>
    /// <param name="id">The ID of the guest to update.</param>
    /// <param name="command">The update command containing the fields to modify.</param>
    /// <response code="204">The guest was successfully updated.</response>
    /// <response code="404">No guest was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateGuest(Guid id, [FromBody] UpdateGuestCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in the route does not match the command ID.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing guest from the system.
    /// </summary>
    /// <remarks>
    /// This endpoint permanently removes the guest and relies on the underlying
    /// data model to enforce or restrict deletion if there are related bookings
    /// or reviews.
    ///
    /// **Route parameter:**
    /// - <c>id</c> – the guest's unique identifier.
    /// </remarks>
    /// <param name="id">The ID of the guest to delete.</param>
    /// <response code="204">Guest was successfully deleted.</response>
    /// <response code="404">No guest was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteGuest(Guid id)
    {
        await _mediator.Send(new DeleteGuestCommand(id));
        return NoContent();
    }
}
