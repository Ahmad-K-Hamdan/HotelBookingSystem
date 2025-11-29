using FluentValidation;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels.Validators;

public class GetHotelsQueryValidator : AbstractValidator<GetHotelsQuery>
{
    public GetHotelsQueryValidator()
    {
        RuleFor(x => x.CheckInDate)
            .NotEmpty().WithMessage("Check-in date is required.")
            .LessThan(x => x.CheckOutDate)
            .WithMessage("Check-in date must be before check-out date.")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Check-in date cannot be in the past.");

        RuleFor(x => x.CheckOutDate)
            .NotEmpty().WithMessage("Check-out date is required.");

        RuleFor(x => x.Rooms)
            .NotNull().WithMessage("Rooms collection cannot be null.");

        RuleForEach(x => x.Rooms)
            .SetValidator(new RoomRequestValidator());

        RuleFor(x => x.Search)
            .MaximumLength(200).WithMessage("Search text must not exceed 200 characters.")
            .Matches("^[A-Za-z0-9 !?,._'\"()\\-/]*$")
            .When(x => !string.IsNullOrWhiteSpace(x.Search))
            .WithMessage("Search contains illegal characters. Only English letters, numbers, and punctuation are allowed.")
            .Must(x => x == null || x.Trim() == x)
            .WithMessage("Search text cannot start or end with spaces.");

        RuleFor(x => x.CityName)
            .Matches("^[A-Za-z ]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.CityName))
            .WithMessage("City name must contain only letters and spaces.")
            .MaximumLength(200).WithMessage("City name must not exceed 200 characters.")
            .Must(x => x == null || x.Trim() == x)
            .WithMessage("City name cannot start or end with spaces.");

        RuleFor(x => x.CountryName)
            .Matches("^[A-Za-z ]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.CountryName))
            .WithMessage("Country name must contain only letters and spaces.").MaximumLength(200).WithMessage("Country name must not exceed 200 characters.")
            .Must(x => x == null || x.Trim() == x)
            .WithMessage("Country name cannot start or end with spaces.");

        RuleFor(x => x.MinStars)
            .InclusiveBetween(1, 5).When(x => x.MinStars.HasValue)
            .WithMessage("Minimum star rating must be between 1 and 5.");

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue)
            .WithMessage("Minimum price cannot be negative.");

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0).When(x => x.MaxPrice.HasValue)
            .WithMessage("Maximum price cannot be negative.");

        RuleFor(x => x)
            .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
            .WithMessage("Minimum price cannot be greater than maximum price.");

        RuleFor(x => x.AmenityIds)
            .Must(list => list == null || list.Distinct().Count() == list.Count)
            .WithMessage("Amenity IDs must not contain duplicates.");

        RuleForEach(x => x.RoomTypes)
            .MaximumLength(100).WithMessage("Room type must not exceed 100 characters.")
            .Matches("^[A-Za-z0-9 !?,._'\"()\\-/]*$")
            .When(x => !string.IsNullOrWhiteSpace(x.Search))
            .WithMessage("Search contains illegal characters. Only English letters, numbers, and punctuation are allowed.")
            .Must(x => x.Trim() == x).WithMessage("Room type cannot start or end with spaces.");

        RuleFor(x => x.Sort)
            .Matches("^(?i)(price|price desc|stars|stars desc|most visited)$")
            .When(x => !string.IsNullOrWhiteSpace(x.Sort))
            .WithMessage("Sort must be one of: 'price', 'price desc', 'stars', 'stars desc', 'most visited'.");

        RuleFor(x => x.Limit)
            .GreaterThan(0).When(x => x.Limit.HasValue)
            .WithMessage("Limit must be greater than 0.");
    }
}
