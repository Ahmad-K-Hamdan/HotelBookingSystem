using FluentAssertions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Features.Hotels.GetHotels;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels.Dtos;
using HotelBookingSystem.Domain.Entities.Bookings;
using HotelBookingSystem.Domain.Entities.Cities;
using HotelBookingSystem.Domain.Entities.Discounts;
using HotelBookingSystem.Domain.Entities.Hotels;
using HotelBookingSystem.Domain.Entities.Rooms;
using HotelBookingSystem.Domain.Entities.Visits;
using MockQueryable.Moq;
using Moq;

namespace HotelBookingSystem.Tests.Hotels;

public class GetHotelsQueryHandlerTests
{
    private readonly Mock<IGenericRepository<Hotel>> _hotelRepo = new();
    private readonly Mock<IGenericRepository<VisitLog>> _visitLogRepo = new();

    private GetHotelsQueryHandler CreateHandler()
        => new(_hotelRepo.Object, _visitLogRepo.Object);

    [Trait("Area", "Hotels")]
    [Trait("Category", "Pricing")]
    [Fact]
    public async Task Handle_ShouldReturnHotel_WhenBasicCriteriaMatch()
    {
        // Arrange
        var (city, hotel, _) = CreateTwoHotelsForDiscountAndAvailability();

        var hotels = new List<Hotel> { hotel };
        var mockHotels = hotels.AsQueryable().BuildMock();
        _hotelRepo.Setup(r => r.Query()).Returns(mockHotels);

        var visitLogs = new List<VisitLog>();
        var mockVisits = visitLogs.AsQueryable().BuildMock();
        _visitLogRepo.Setup(r => r.Query()).Returns(mockVisits);

        var query = new GetHotelsQuery
        {
            CheckInDate = DateOnly.FromDateTime(DateTime.Today),
            CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            Rooms = new List<RoomRequest>()
        };

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        var dto = result.Single();

        dto.HotelName.Should().Be(hotel.HotelName);
        dto.CityName.Should().Be(city.CityName);
        dto.CountryName.Should().Be(city.CountryName);
        dto.StarRating.Should().Be(hotel.StarRating);
        dto.MinTotalOriginalPricePerNight.Should().Be(100m);
        dto.MinTotalDiscountedPricePerNight.Should().Be(90m); // 10% off
    }

    [Trait("Area", "Hotels")]
    [Trait("Category", "Filtering")]
    [Fact]
    public async Task Handle_ShouldReturnOnlyHotelsWithActiveDiscount_WhenOnlyWithActiveDiscountIsTrue()
    {
        // Arrange
        var (_, hotelWithDiscount, hotelWithoutDiscount) = CreateTwoHotelsForDiscountAndAvailability();

        var hotels = new List<Hotel> { hotelWithDiscount, hotelWithoutDiscount };
        var mockHotels = hotels.AsQueryable().BuildMock();
        _hotelRepo.Setup(r => r.Query()).Returns(mockHotels);

        var visitLogs = new List<VisitLog>();
        var mockVisits = visitLogs.AsQueryable().BuildMock();
        _visitLogRepo.Setup(r => r.Query()).Returns(mockVisits);

        var query = new GetHotelsQuery
        {
            CheckInDate = DateOnly.FromDateTime(DateTime.Today),
            CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            OnlyWithActiveDiscount = true
        };

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        var dto = result.Single();
        dto.HotelName.Should().Be("With Discount");
        dto.HasActiveDiscount.Should().BeTrue();
    }

    [Trait("Area", "Hotels")]
    [Trait("Category", "Availability")]
    [Fact]
    public async Task Handle_ShouldExcludeHotelsWhereNoRoomsAreAvailableForGivenDatesAndCapacity()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.Today);
        var tomorrow = today.AddDays(1);

        var (_, hotelWithDiscount, hotelWithoutDiscount) = CreateTwoHotelsForDiscountAndAvailability();

        // Make the room in the discounted hotel fully booked for the requested date range
        var bookedRoom = hotelWithDiscount.RoomTypes.First().Rooms.First();

        var overlappingBooking = new Booking
        {
            Id = Guid.NewGuid(),
            HotelId = hotelWithDiscount.Id,
            CheckInDate = today,
            CheckOutDate = today.AddDays(2), // overlaps
            Nights = 2,
            TotalAdults = 2,
            TotalChildren = 1,
            ConfirmationCode = "TEST-BOOKING"
        };

        bookedRoom.BookingRooms.Add(new BookingRoom
        {
            Id = Guid.NewGuid(),
            Booking = overlappingBooking,
            BookingId = overlappingBooking.Id,
            HotelRoomId = bookedRoom.Id,
            NumOfAdults = 2,
            NumOfChildren = 1,
            PricePerNightOriginal = 100m,
            PricePerNightDiscounted = 90m
        });

        var hotels = new List<Hotel> { hotelWithDiscount, hotelWithoutDiscount };
        var mockHotels = hotels.AsQueryable().BuildMock();
        _hotelRepo.Setup(r => r.Query()).Returns(mockHotels);

        var query = new GetHotelsQuery
        {
            CheckInDate = today,
            CheckOutDate = tomorrow,
            Rooms = new List<RoomRequest>
            {
                new()
                {
                    Adults = 2,
                    Children = 1
                }
            }
        };

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        // Discounted hotel is fully booked for that date -> excluded,
        // non-discounted hotel has a free room with enough capacity -> included.
        result.Should().HaveCount(1);
        var dto = result.Single();
        dto.HotelName.Should().Be("Without Discount");
    }

    private static (City city, Hotel hotelWithDiscount, Hotel hotelWithoutDiscount) CreateTwoHotelsForDiscountAndAvailability()
    {
        var city = new City
        {
            Id = Guid.NewGuid(),
            CityName = "Gaza",
            CountryName = "Palestine"
        };

        var activeDiscount = new Discount
        {
            Id = Guid.NewGuid(),
            DiscountRate = 0.1m,
            IsActive = true,
            DiscountDescription = "10% off"
        };

        var inactiveDiscount = new Discount
        {
            Id = Guid.NewGuid(),
            DiscountRate = 0.2m,
            IsActive = false,
            DiscountDescription = "20% off"
        };

        var hotelWithDiscount = new Hotel
        {
            Id = Guid.NewGuid(),
            HotelName = "With Discount",
            HotelAddress = "Street 1",
            City = city,
            CityId = city.Id,
            StarRating = 4,
            HotelGroup = new HotelGroup { Id = Guid.NewGuid(), GroupName = "Group A" },
            Discount = activeDiscount,
            RoomTypes = new List<HotelRoomType>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Std",
                    PricePerNight = 100m,
                    MaxNumOfGuestsAdults = 3,
                    MaxNumOfGuestsChildren = 1,
                    Rooms = new List<HotelRoom>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            RoomNumber = 101,
                            IsAvailable = true,
                            BookingRooms = new List<BookingRoom>()
                        }
                    }
                }
            },
            Images = new List<HotelImage>(),
            HotelAmenities = new List<HotelAmenity>()
        };

        var hotelWithoutDiscount = new Hotel
        {
            Id = Guid.NewGuid(),
            HotelName = "Without Discount",
            HotelAddress = "Street 2",
            City = city,
            CityId = city.Id,
            StarRating = 3,
            HotelGroup = new HotelGroup { Id = Guid.NewGuid(), GroupName = "Group B" },
            Discount = inactiveDiscount,
            RoomTypes = new List<HotelRoomType>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Std",
                    PricePerNight = 120m,
                    MaxNumOfGuestsAdults = 3,
                    MaxNumOfGuestsChildren = 1,
                    Rooms = new List<HotelRoom>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            RoomNumber = 1201,
                            IsAvailable = true,
                            BookingRooms = new List<BookingRoom>()
                        }
                    }
                }
            },
            Images = new List<HotelImage>(),
            HotelAmenities = new List<HotelAmenity>()
        };

        return (city, hotelWithDiscount, hotelWithoutDiscount);
    }
}
