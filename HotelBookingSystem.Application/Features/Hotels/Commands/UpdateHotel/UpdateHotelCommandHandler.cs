using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Cities;
using HotelBookingSystem.Domain.Entities.Discounts;
using HotelBookingSystem.Domain.Entities.Hotels;
using MediatR;

namespace HotelBookingSystem.Application.Features.Hotels.Commands.UpdateHotel;

public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, Unit>
{
    private readonly IGenericRepository<Hotel> _hotelRepository;
    private readonly IGenericRepository<City> _cityRepository;
    private readonly IGenericRepository<HotelGroup> _hotelGroupRepository;
    private readonly IGenericRepository<Discount> _discountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHotelCommandHandler(
        IGenericRepository<Hotel> hotelRepository,
        IGenericRepository<City> cityRepository,
        IGenericRepository<HotelGroup> hotelGroupRepository,
        IGenericRepository<Discount> discountRepository,
        IUnitOfWork unitOfWork)
    {
        _hotelRepository = hotelRepository;
        _cityRepository = cityRepository;
        _hotelGroupRepository = hotelGroupRepository;
        _discountRepository = discountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.Id);

        if (hotel == null)
        {
            throw new NotFoundException(nameof(hotel), request.Id);
        }

        _ = await _cityRepository.GetByIdAsync(request.Hotel.CityId)
            ?? throw new NotFoundException("City", request.Hotel.CityId);

        _ = await _hotelGroupRepository.GetByIdAsync(request.Hotel.HotelGroupId)
            ?? throw new NotFoundException("Hotel group", request.Hotel.HotelGroupId);

        if (request.Hotel.DiscountId is Guid discountId)
        {
            _ = await _discountRepository.GetByIdAsync(discountId)
                ?? throw new NotFoundException("Discount", discountId);
        }

        hotel.HotelGroupId = request.Hotel.HotelGroupId;
        hotel.CityId = request.Hotel.CityId;
        hotel.DiscountId = request.Hotel.DiscountId;
        hotel.HotelName = request.Hotel.HotelName;
        hotel.HotelAddress = request.Hotel.HotelAddress;
        hotel.StarRating = request.Hotel.StarRating;
        hotel.Latitude = request.Hotel.Latitude;
        hotel.Longitude = request.Hotel.Longitude;
        hotel.Description = request.Hotel.Description;

        _hotelRepository.Update(hotel);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
