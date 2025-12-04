using FluentAssertions;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Features.Bookings.Commands.CreateBooking;
using HotelBookingSystem.Domain.Entities.Bookings;
using HotelBookingSystem.Domain.Entities.Guests;
using HotelBookingSystem.Domain.Entities.Hotels;
using HotelBookingSystem.Domain.Entities.Rooms;
using MockQueryable.Moq;
using Moq;
using System.Linq.Expressions;

namespace HotelBookingSystem.Tests.Bookings;

public class CreateBookingCommandHandlerTests
{
    private readonly Mock<IGenericRepository<Booking>> _bookingRepository = new();
    private readonly Mock<IGenericRepository<BookingRoom>> _bookingRoomRepository = new();
    private readonly Mock<IGenericRepository<HotelRoomType>> _roomTypeRepository = new();
    private readonly Mock<IGenericRepository<Guest>> _guestRepository = new();
    private readonly Mock<IGenericRepository<Hotel>> _hotelRepository = new();
    private readonly Mock<ICurrentUserService> _currentUserService = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    private CreateBookingCommandHandler CreateHandler()
        => new(_bookingRepository.Object, _bookingRoomRepository.Object,
            _roomTypeRepository.Object, _guestRepository.Object, _hotelRepository.Object,
            _currentUserService.Object, _unitOfWork.Object);

    [Trait("Area", "Bookings")]
    [Trait("Category", "Authentication")]
    [Fact]
    public async Task Handle_ShouldThrowUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        _currentUserService.Setup(x => x.UserId).Returns((string?)null);

        var command = new CreateBookingCommand
        {
            CheckInDate = DateOnly.FromDateTime(DateTime.Today),
            CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            Rooms = new List<BookingRoomRequestDto>
                {
                    new()
                    {
                        HotelRoomTypeId = Guid.NewGuid(),
                        Adults = 2,
                        Children = 0
                    }
                }
        };

        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("User not authenticated.");
    }

    [Trait("Area", "Bookings")]
    [Trait("Category", "Validation")]
    [Fact]
    public async Task Handle_ShouldThrowValidation_WhenGuestProfileMissing()
    {
        // Arrange
        _currentUserService.Setup(x => x.UserId).Returns("user-123");

        // guest repository returns empty list
        _guestRepository.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Guest, bool>>>())).ReturnsAsync(new List<Guest>());

        var command = new CreateBookingCommand
        {
            CheckInDate = DateOnly.FromDateTime(DateTime.Today),
            CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            Rooms = new List<BookingRoomRequestDto>
                {
                    new()
                    {
                        HotelRoomTypeId = Guid.NewGuid(),
                        Adults = 2,
                        Children = 0
                    }
                }
        };

        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors != null && ex.Errors.Any(e => e.PropertyName == "Guest"));
    }

    [Trait("Area", "Bookings")]
    [Trait("Category", "BusinessRules")]
    [Fact]
    public async Task Handle_ShouldThrowValidation_WhenRoomTypesBelongToDifferentHotels()
    {
        // Arrange
        var userId = "user-123";
        _currentUserService.Setup(x => x.UserId).Returns(userId);

        _guestRepository.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Guest, bool>>>()))
            .ReturnsAsync(new List<Guest>
            {
                new() { Id = Guid.NewGuid(), UserId = userId, HomeCountry = "Palestine" }
            });

        var roomTypeId1 = Guid.NewGuid();
        var roomTypeId2 = Guid.NewGuid();

        var hotelA = new Hotel { Id = Guid.NewGuid(), HotelName = "Hotel A" };
        var hotelB = new Hotel { Id = Guid.NewGuid(), HotelName = "Hotel B" };

        var roomTypes = new List<HotelRoomType>
        {
            new()
            {
                Id = roomTypeId1,
                HotelId = hotelA.Id,
                Hotel = hotelA,
                Name = "Type A",
                Rooms = new List<HotelRoom>()
            },
            new()
            {
                Id = roomTypeId2,
                HotelId = hotelB.Id,
                Hotel = hotelB,
                Name = "Type B",
                Rooms = new List<HotelRoom>()
            }
        };

        var mockRoomTypes = roomTypes.AsQueryable().BuildMock();
        _roomTypeRepository.Setup(r => r.Query()).Returns(mockRoomTypes);

        var command = new CreateBookingCommand
        {
            CheckInDate = DateOnly.FromDateTime(DateTime.Today),
            CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            Rooms = new List<BookingRoomRequestDto>
            {
                new() { HotelRoomTypeId = roomTypeId1, Adults = 2, Children = 0 },
                new() { HotelRoomTypeId = roomTypeId2, Adults = 2, Children = 0 }
            }
        };

        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.Any(e => e.PropertyName == "Rooms"));
    }

    [Trait("Area", "Bookings")]
    [Trait("Category", "ValidRequest")]
    [Fact]
    public async Task Handle_ShouldCreateBooking_WhenRequestIsValid()
    {
        // Arrange
        var userId = "user-123";
        _currentUserService.Setup(x => x.UserId).Returns(userId);

        var guest = new Guest
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            HomeCountry = "Palestine"
        };

        _guestRepository.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Guest, bool>>>())).ReturnsAsync(new List<Guest> { guest });

        var hotel = new Hotel { Id = Guid.NewGuid(), HotelName = "Test Hotel" };
        var roomTypeId = Guid.NewGuid();
        var roomType = new HotelRoomType
        {
            Id = roomTypeId,
            HotelId = hotel.Id,
            Hotel = hotel,
            Name = "Standard",
            PricePerNight = 100m,
            MaxNumOfGuestsAdults = 2,
            MaxNumOfGuestsChildren = 1,
            Rooms = new List<HotelRoom>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    HotelRoomTypeId = roomTypeId,
                    RoomNumber = 101,
                    IsAvailable = true,
                    BookingRooms = new List<BookingRoom>()
                }
            }
        };

        var roomTypes = new List<HotelRoomType> { roomType };
        var mockRoomTypes = roomTypes.AsQueryable().BuildMock();

        _roomTypeRepository.Setup(r => r.Query()).Returns(mockRoomTypes);
        _hotelRepository.Setup(r => r.GetByIdAsync(hotel.Id)).ReturnsAsync(hotel);
        _unitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        var command = new CreateBookingCommand
        {
            CheckInDate = DateOnly.FromDateTime(DateTime.Today),
            CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
            Rooms = new List<BookingRoomRequestDto>
            {
                new() { HotelRoomTypeId = roomTypeId, Adults = 2, Children = 0 }
            },
            SpecialRequests = "High floor"
        };

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);

        _bookingRepository.Verify(r => r.AddAsync(It.Is<Booking>(b =>
            b.GuestId == guest.Id &&
            b.HotelId == hotel.Id &&
            b.Nights == 2 &&
            b.TotalAdults == 2 &&
            b.TotalChildren == 0 &&
            b.SpecialRequests == "High floor"
        )), Times.Once);

        _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
