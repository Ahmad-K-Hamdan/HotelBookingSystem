using FluentAssertions;
using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Features.Payments.Commands.CreatePaymentForBooking;
using HotelBookingSystem.Domain.Entities.Bookings;
using HotelBookingSystem.Domain.Entities.Guests;
using HotelBookingSystem.Domain.Entities.Hotels;
using HotelBookingSystem.Domain.Entities.Payments;
using HotelBookingSystem.Domain.Enums;
using Moq;
using MockQueryable.Moq;

namespace HotelBookingSystem.Tests.Payments;

public class CreatePaymentForBookingCommandHandlerTests
{
    private readonly Mock<IGenericRepository<Booking>> _bookingRepo = new();
    private readonly Mock<IGenericRepository<Payment>> _paymentRepo = new();
    private readonly Mock<IGenericRepository<PaymentMethod>> _paymentMethodRepo = new();
    private readonly Mock<ICurrentUserService> _currentUserService = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IIdentityService> _identityService = new();
    private readonly Mock<IEmailService> _emailService = new();
    private readonly Mock<IPaymentEmailTemplateService> _templateService = new();

    private CreatePaymentForBookingCommandHandler CreateHandler()
        => new(_bookingRepo.Object, _paymentRepo.Object,
            _paymentMethodRepo.Object, _currentUserService.Object,
            _unitOfWork.Object, _identityService.Object,
            _emailService.Object, _templateService.Object);

    [Trait("Area", "Payments")]
    [Trait("Category", "Authentication")]
    [Fact]
    public async Task Handle_ShouldThrowUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        _currentUserService.Setup(x => x.UserId).Returns((string?)null);

        var command = new CreatePaymentForBookingCommand
        {
            BookingId = Guid.NewGuid(),
            PaymentMethodId = Guid.NewGuid(),
            Amount = 100m
        };

        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("User not authenticated.");
    }

    [Trait("Area", "Payments")]
    [Trait("Category", "BusinessRules")]
    [Fact]
    public async Task Handle_ShouldThrowValidation_WhenAmountExceedsRemaining()
    {
        // Arrange
        var userId = "user-123";
        _currentUserService.Setup(x => x.UserId).Returns(userId);

        var bookingId = Guid.NewGuid();
        var hotel = new Hotel { Id = Guid.NewGuid() };

        var guest = new Guest
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            HomeCountry = "Palestine"
        };

        var existingPayment = new Payment
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            PaymentAmount = 50m,
            PaymentStatus = PaymentStatus.Completed
        };

        var booking = new Booking
        {
            Id = bookingId,
            Guest = guest,
            Hotel = hotel,
            TotalDiscountedPrice = 100m,
            Payments = new List<Payment> { existingPayment }
        };

        var bookings = new List<Booking> { booking };
        var mockBookings = bookings.AsQueryable().BuildMock();

        _bookingRepo.Setup(r => r.Query()).Returns(mockBookings);

        _paymentMethodRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new PaymentMethod
            {
                Id = Guid.NewGuid(),
                MethodName = "Visa"
            });

        var command = new CreatePaymentForBookingCommand
        {
            BookingId = bookingId,
            PaymentMethodId = Guid.NewGuid(),
            Amount = 60m // 100 total, already paid 50 -> remaining 50 → 60 is too much
        };

        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.Any(e => e.PropertyName == "Amount"));
    }

    [Trait("Area", "Payments")]
    [Trait("Category", "ValidRequest")]
    [Fact]
    public async Task Handle_ShouldCreatePayment_WhenRequestIsValid()
    {
        // Arrange
        var userId = "user-123";
        _currentUserService.Setup(x => x.UserId).Returns(userId);

        var bookingId = Guid.NewGuid();
        var hotel = new Hotel { Id = Guid.NewGuid(), HotelName = "Hotel", DiscountId = null };
        var guest = new Guest { Id = Guid.NewGuid(), UserId = userId, HomeCountry = "Palestine" };

        var existingPayment = new Payment
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            PaymentAmount = 30m,
            PaymentStatus = PaymentStatus.Completed
        };

        var booking = new Booking
        {
            Id = bookingId,
            Guest = guest,
            Hotel = hotel,
            TotalDiscountedPrice = 100m,
            Payments = new List<Payment> { existingPayment }
        };

        var bookings = new List<Booking> { booking };
        var mockBookings = bookings.AsQueryable().BuildMock();

        _bookingRepo.Setup(r => r.Query()).Returns(mockBookings);
        _unitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        _paymentMethodRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new PaymentMethod { Id = Guid.NewGuid(), MethodName = "Visa" });

        var command = new CreatePaymentForBookingCommand
        {
            BookingId = bookingId,
            PaymentMethodId = Guid.NewGuid(),
            Amount = 50m
        };

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);

        _paymentRepo.Verify(r => r.AddAsync(It.Is<Payment>(p =>
            p.BookingId == bookingId &&
            p.PaymentAmount == 50m &&
            p.PaymentStatus == PaymentStatus.Completed
        )), Times.Once);

        _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
