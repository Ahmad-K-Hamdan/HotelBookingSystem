using FluentValidation;
using FluentValidation.Results;
using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Bookings;
using HotelBookingSystem.Domain.Entities.Guests;
using HotelBookingSystem.Domain.Entities.Hotels;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly IGenericRepository<Booking> _bookingRepository;
    private readonly IGenericRepository<BookingRoom> _bookingRoomRepository;
    private readonly IGenericRepository<HotelRoomType> _roomTypeRepository;
    private readonly IGenericRepository<HotelRoom> _roomRepository;
    private readonly IGenericRepository<Guest> _guestRepository;
    private readonly IGenericRepository<Hotel> _hotelRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingCommandHandler(
        IGenericRepository<Booking> bookingRepository,
        IGenericRepository<BookingRoom> bookingRoomRepository,
        IGenericRepository<HotelRoomType> roomTypeRepository,
        IGenericRepository<HotelRoom> roomRepository,
        IGenericRepository<Guest> guestRepository,
        IGenericRepository<Hotel> hotelRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _bookingRoomRepository = bookingRoomRepository;
        _roomTypeRepository = roomTypeRepository;
        _roomRepository = roomRepository;
        _guestRepository = guestRepository;
        _hotelRepository = hotelRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new UnauthorizedAccessException("User not authenticated.");
        }

        var guest = (await _guestRepository.FindAsync(g => g.UserId == userId)).FirstOrDefault();
        if (guest is null)
        {
            throw new ValidationException(
            [
                new ValidationFailure("Guest", "Guest profile is required before creating a booking.")
            ]);
        }

        var requestedTypeIds = request.Rooms
            .Select(r => r.HotelRoomTypeId)
            .Distinct()
            .ToList();

        var roomTypes = await _roomTypeRepository.Query()
            .Where(rt => requestedTypeIds.Contains(rt.Id))
            .Include(rt => rt.Hotel)
                .ThenInclude(h => h.Discount)
            .Include(rt => rt.Rooms)
                .ThenInclude(r => r.BookingRooms)
                    .ThenInclude(br => br.Booking)
            .ToListAsync(cancellationToken);

        if (roomTypes.Count != requestedTypeIds.Count)
        {
            throw new NotFoundException("Hotel room type", requestedTypeIds);
        }

        var hotelIds = roomTypes.Select(rt => rt.HotelId).Distinct().ToList();
        if (hotelIds.Count != 1)
        {
            throw new ValidationException(
            [
                new ValidationFailure("Rooms", "All selected room types must belong to the same hotel.")
            ]);
        }

        var hotelId = hotelIds[0];
        var hotel = await _hotelRepository.GetByIdAsync(hotelId)
            ?? throw new NotFoundException("Hotel", hotelId);

        var discountFactor = hotel.Discount != null && hotel.Discount.IsActive
            ? 1 - hotel.Discount.DiscountRate
            : 1m;

        var availableRoomsByType = roomTypes.ToDictionary(
            rt => rt.Id,
            rt => rt.Rooms
                .Where(r => r.IsAvailable && RoomIsFree(r, request.CheckInDate, request.CheckOutDate))
                .OrderBy(r => r.RoomNumber)
                .ToList());

        var assigned = new List<(HotelRoom Room, HotelRoomType Type, int Adults, int Children)>();

        for (var i = 0; i < request.Rooms.Count; i++)
        {
            var req = request.Rooms[i];

            if (!availableRoomsByType.TryGetValue(req.HotelRoomTypeId, out var roomList) || roomList.Count == 0)
            {
                throw new ValidationException(
                [
                    new ValidationFailure(
                        $"Rooms[{i}].HotelRoomTypeId",
                        $"No available room for the selected type ({req.HotelRoomTypeId}) and dates.")
                ]);
            }

            var type = roomTypes.Single(rt => rt.Id == req.HotelRoomTypeId);

            if (req.Adults > type.MaxNumOfGuestsAdults)
            {
                throw new ValidationException(
                [
                    new ValidationFailure(
                        $"Rooms[{i}].Adults",
                        $"Requested adults ({req.Adults}) exceeds capacity ({type.MaxNumOfGuestsAdults}) for room type '{type.Name}'.")
                ]);
            }

            if (req.Children > type.MaxNumOfGuestsChildren)
            {
                throw new ValidationException(
                [
                    new ValidationFailure(
                        $"Rooms[{i}].Children",
                        $"Requested children ({req.Children}) exceeds capacity ({type.MaxNumOfGuestsChildren}) for room type '{type.Name}'.")
                ]);
            }

            var room = roomList[0];
            roomList.RemoveAt(0);

            assigned.Add((room, type, req.Adults, req.Children));
        }

        var totalAdults = assigned.Sum(x => x.Adults);
        var totalChildren = assigned.Sum(x => x.Children);

        var totalOriginalPerNight = assigned.Sum(x => x.Type.PricePerNight);
        var totalDiscountedPerNight = totalOriginalPerNight * discountFactor;

        var nights = request.CheckOutDate.DayNumber - request.CheckInDate.DayNumber;
        var totalOriginal = totalOriginalPerNight * nights;
        var totalDiscounted = totalDiscountedPerNight * nights;

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            GuestId = guest.Id,
            HotelId = hotelId,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            Nights = nights,
            TotalAdults = totalAdults,
            TotalChildren = totalChildren,
            SpecialRequests = request.SpecialRequests,
            ConfirmationCode = GenerateConfirmationCode(hotel, guest),
            CreatedAt = DateTime.UtcNow,
            TotalOriginalPrice = totalOriginal,
            TotalDiscountedPrice = totalDiscounted
        };

        await _bookingRepository.AddAsync(booking);

        foreach (var a in assigned)
        {
            var br = new BookingRoom
            {
                Id = Guid.NewGuid(),
                BookingId = booking.Id,
                HotelRoomId = a.Room.Id,
                NumOfAdults = a.Adults,
                NumOfChildren = a.Children,
                PricePerNightOriginal = a.Type.PricePerNight,
                PricePerNightDiscounted = a.Type.PricePerNight * discountFactor
            };

            await _bookingRoomRepository.AddAsync(br);
        }

        await _unitOfWork.SaveChangesAsync();
        return booking.Id;
    }

    private static bool RoomIsFree(HotelRoom room, DateOnly checkIn, DateOnly checkOut)
        => !room.BookingRooms.Any(br =>
            br.Booking.CheckInDate < checkOut &&
            br.Booking.CheckOutDate > checkIn);

    private static string GenerateConfirmationCode(Hotel hotel, Guest guest)
    {
        var prefix = hotel.HotelName.Length >= 3
            ? hotel.HotelName[..3].ToUpperInvariant()
            : hotel.HotelName.ToUpperInvariant();

        return $"{prefix}-{guest.Id.ToString()[..8]}-{DateTime.UtcNow:yyyyMMddHHmmss}";
    }
}
