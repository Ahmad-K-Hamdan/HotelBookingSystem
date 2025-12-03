using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Amenities;
using HotelBookingSystem.Domain.Entities.Cities;
using HotelBookingSystem.Domain.Entities.Discounts;
using HotelBookingSystem.Domain.Entities.Hotels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Hotels.Commands.UpdateHotel;

public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, Unit>
{
    private readonly IGenericRepository<Hotel> _hotelRepository;
    private readonly IGenericRepository<City> _cityRepository;
    private readonly IGenericRepository<HotelGroup> _hotelGroupRepository;
    private readonly IGenericRepository<Amenity> _amenityRepository;
    private readonly IGenericRepository<HotelAmenity> _hotelAmenityRepository;
    private readonly IGenericRepository<Discount> _discountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHotelCommandHandler(
        IGenericRepository<Hotel> hotelRepository,
        IGenericRepository<City> cityRepository,
        IGenericRepository<HotelGroup> hotelGroupRepository,
        IGenericRepository<Amenity> amenityRepository,
        IGenericRepository<HotelAmenity> hotelAmenityRepository,
        IGenericRepository<Discount> discountRepository,
        IUnitOfWork unitOfWork)
    {
        _hotelRepository = hotelRepository;
        _cityRepository = cityRepository;
        _hotelGroupRepository = hotelGroupRepository;
        _amenityRepository = amenityRepository;
        _hotelAmenityRepository = hotelAmenityRepository;
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

        var requestedAmenityIds = request.Hotel.AmenityIds?
           .Distinct()
           .ToList() ?? new List<Guid>();

        if (requestedAmenityIds.Count > 0)
        {
            var existingAmenityIds = await _amenityRepository.Query()
                .Where(a => requestedAmenityIds.Contains(a.Id))
                .Select(a => a.Id)
                .ToListAsync(cancellationToken);

            var missing = requestedAmenityIds.Except(existingAmenityIds).ToList();
            if (missing.Count > 0)
            {
                throw new NotFoundException("Amenity", string.Join(", ", missing));
            }
        }

        var currentLinks = await _hotelAmenityRepository.Query()
            .Where(ha => ha.HotelId == request.Id)
            .ToListAsync(cancellationToken);

        var currentIds = currentLinks.Select(ha => ha.AmenityId).ToList();

        var toAdd = requestedAmenityIds.Except(currentIds).ToList();
        var toRemove = currentLinks
            .Where(ha => !requestedAmenityIds.Contains(ha.AmenityId))
            .ToList();

        foreach (var link in toRemove)
        {
            _hotelAmenityRepository.Delete(link);
        }

        foreach (var amenityId in toAdd)
        {
            await _hotelAmenityRepository.AddAsync(new HotelAmenity
            {
                HotelId = hotel.Id,
                AmenityId = amenityId
            });
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
