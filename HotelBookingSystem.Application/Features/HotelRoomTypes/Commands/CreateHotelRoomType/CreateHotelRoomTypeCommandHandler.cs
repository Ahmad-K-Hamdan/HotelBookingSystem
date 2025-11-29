using AutoMapper;
using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Hotels;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Commands.CreateHotelRoomType;

public class CreateHotelRoomTypeCommandHandler : IRequestHandler<CreateHotelRoomTypeCommand, Guid>
{
    private readonly IGenericRepository<HotelRoomType> _roomTypeRepository;
    private readonly IGenericRepository<Hotel> _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateHotelRoomTypeCommandHandler(
        IGenericRepository<HotelRoomType> roomTypeRepository,
        IGenericRepository<Hotel> hotelRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _roomTypeRepository = roomTypeRepository;
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateHotelRoomTypeCommand request, CancellationToken cancellationToken)
    {
        var roomTypeDto = request.HotelRoomType;

        _ = await _hotelRepository.GetByIdAsync(roomTypeDto.HotelId)
            ?? throw new NotFoundException("Hotel", roomTypeDto.HotelId);

        var roomType = _mapper.Map<HotelRoomType>(roomTypeDto);
        roomType.Id = Guid.NewGuid();

        await _roomTypeRepository.AddAsync(roomType);
        await _unitOfWork.SaveChangesAsync();
        return roomType.Id;
    }
}
