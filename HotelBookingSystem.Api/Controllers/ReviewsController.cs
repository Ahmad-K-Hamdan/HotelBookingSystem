using HotelBookingSystem.Application.Features.Reviews.Commands.CreateReview;
using HotelBookingSystem.Application.Features.Reviews.Commands.DeleteReview;
using HotelBookingSystem.Application.Features.Reviews.Commands.UpdateReview;
using HotelBookingSystem.Application.Features.Reviews.Queries.GetReviewById;
using HotelBookingSystem.Application.Features.Reviews.Queries.GetReviewById.Dtos;
using HotelBookingSystem.Application.Features.Reviews.Queries.GetReviews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// API controller responsible for managing hotel reviews.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the ReviewsController class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all reviews in the system.
    /// </summary>
    /// <remarks>
    /// This endpoint is typically used for administrative or reporting views.
    ///
    /// **Returned <c>ReviewListDto</c> includes:**
    /// - <c>Id</c> – review identifier.
    /// - <c>HotelId</c>, <c>HotelName</c>.
    /// - <c>GuestId</c>.
    /// - <c>Rating</c>, <c>Comment</c>, <c>ReviewDate</c>.
    /// </remarks>
    /// <response code="200">Successfully returned the list of reviews.</response>
    [HttpGet]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<ReviewListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReviews()
        => Ok(await _mediator.Send(new GetReviewsQuery()));

    /// <summary>
    /// Retrieves a single review by its ID, including minimal hotel and guest information.
    /// </summary>
    /// <remarks>
    /// Use this endpoint when you want to show the details of a specific review,
    /// for example in a review detail page or admin detail view.
    ///
    /// **Route parameter:**
    /// - <c>id</c> – the review's unique identifier.
    ///
    /// **Returned <c>ReviewDetailsDto</c> includes:**
    /// - Review info: <c>Id</c>, <c>Rating</c>, <c>Comment</c>, <c>ReviewDate</c>.
    /// - <c>Hotel</c> – minimal hotel information:
    ///   - <c>Id</c>, <c>HotelName</c>, <c>CityName</c>, <c>CountryName</c>, <c>StarRating</c>.
    /// - <c>Guest</c> – minimal guest information:
    ///   - <c>Id</c>, <c>UserId</c>, <c>HomeCountry</c>.
    /// </remarks>
    /// <param name="id">The ID of the review to retrieve.</param>
    /// <response code="200">Successfully returned the review details.</response>
    /// <response code="404">No review was found with the given ID.</response>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ReviewDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReviewById(Guid id)
        => Ok(await _mediator.Send(new GetReviewByIdQuery(id)));

    /// <summary>
    /// [Authenticated] Creates a new review for a hotel.
    /// </summary>
    /// <remarks>
    /// This endpoint allows a guest to submit a review for a specific hotel.
    /// 
    /// **Request body (<c>CreateReviewCommand</c>) includes:**
    /// - <c>HotelId</c> – the hotel being reviewed (must exist).
    /// - <c>GuestId</c> – the guest who is submitting the review (must exist).
    /// - <c>Rating</c> – an integer between 1 and 5.
    /// - <c>Comment</c> – optional text (up to 1000 characters).
    /// </remarks>
    /// <param name="command">The command containing review creation details.</param>
    /// <response code="200">Review was successfully created and the new ID was returned.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetReviewById), new { id }, id);
    }

    /// <summary>
    /// [Authenticated] Updates an existing review.
    /// </summary>
    /// <remarks>
    /// This endpoint allows updating the review's rating and comment.
    /// The review's <c>HotelId</c> and <c>GuestId</c> cannot be changed.
    ///
    /// **Route parameter:**
    /// - <c>id</c> – the ID of the review to update.
    ///
    /// **Request body (<c>UpdateReviewCommand</c>) includes:**
    /// - <c>Id</c> – must match the route ID.
    /// - <c>Rating</c> – new rating (1–5).
    /// - <c>Comment</c> – new comment (optional, up to 1000 characters).
    /// </remarks>
    /// <param name="id">The ID of the review to update.</param>
    /// <param name="command">The command containing updated review details.</param>
    /// <response code="204">Review was successfully updated.</response>
    /// <response code="404">No review was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateReview(Guid id, [FromBody] UpdateReviewCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in route does not match command ID.");
        }

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// [Authenticated] Deletes an existing review from the system.
    /// </summary>
    /// <remarks>
    /// **Route parameter:**
    /// - <c>id</c> – the ID of the review to delete.
    /// </remarks>
    /// <param name="id">The ID of the review to delete.</param>
    /// <response code="204">Review was successfully deleted.</response>
    /// <response code="404">No review was found with the given ID.</response>
    /// <response code="400">The request was invalid.</response>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteReview(Guid id)
    {
        await _mediator.Send(new DeleteReviewCommand(id));
        return NoContent();
    }
}
