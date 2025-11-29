using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Hotels;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Commands.UpdateHotelRoomType;

public class UpdateHotelRoomTypeCommandHandler : IRequestHandler<UpdateHotelRoomTypeCommand, Unit>
{
    private readonly IGenericRepository<HotelRoomType> _roomTypeRepository;
    private readonly IGenericRepository<Hotel> _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHotelRoomTypeCommandHandler(
        IGenericRepository<HotelRoomType> roomTypeRepository,
        IGenericRepository<Hotel> hotelRepository,
        IUnitOfWork unitOfWork)
    {
        _roomTypeRepository = roomTypeRepository;
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateHotelRoomTypeCommand request, CancellationToken cancellationToken)
    {
        var roomType = await _roomTypeRepository.GetByIdAsync(request.Id);

        if (roomType is null)
        {
            throw new NotFoundException(nameof(HotelRoomType), request.Id);
        }

        var roomTypeDto = request.HotelRoomType;

        _ = await _hotelRepository.GetByIdAsync(roomTypeDto.HotelId)
            ?? throw new NotFoundException("Hotel", roomTypeDto.HotelId);

        roomType.HotelId = roomTypeDto.HotelId;
        roomType.Name = roomTypeDto.Name.Trim();
        roomType.Description = roomTypeDto.Description?.Trim();
        roomType.PricePerNight = roomTypeDto.PricePerNight;
        roomType.BedsCount = roomTypeDto.BedsCount;
        roomType.MaxNumOfGuestsAdults = roomTypeDto.MaxNumOfGuestsAdults;
        roomType.MaxNumOfGuestsChildren = roomTypeDto.MaxNumOfGuestsChildren;

        _roomTypeRepository.Update(roomType);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
