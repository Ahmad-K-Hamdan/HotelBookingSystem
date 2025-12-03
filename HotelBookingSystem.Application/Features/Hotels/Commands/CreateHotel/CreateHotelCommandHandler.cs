using AutoMapper;
using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Amenities;
using HotelBookingSystem.Domain.Entities.Cities;
using HotelBookingSystem.Domain.Entities.Discounts;
using HotelBookingSystem.Domain.Entities.Hotels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Hotels.Commands.CreateHotel;

public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Guid>
{
    private readonly IGenericRepository<Hotel> _hotelRepository;
    private readonly IGenericRepository<City> _cityRepository;
    private readonly IGenericRepository<HotelGroup> _hotelGroupRepository;
    private readonly IGenericRepository<Amenity> _amenityRepository;          
    private readonly IGenericRepository<HotelAmenity> _hotelAmenityRepository;
    private readonly IGenericRepository<Discount> _discountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateHotelCommandHandler(
        IGenericRepository<Hotel> hotelRepository,
        IGenericRepository<City> cityRepository,
        IGenericRepository<HotelGroup> hotelGroupRepository,
        IGenericRepository<Amenity> amenityRepository,
        IGenericRepository<HotelAmenity> hotelAmenityRepository,
        IGenericRepository<Discount> discountRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _cityRepository = cityRepository;
        _hotelGroupRepository = hotelGroupRepository;
        _amenityRepository = amenityRepository;
        _hotelAmenityRepository = hotelAmenityRepository;
        _discountRepository = discountRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotelDto = request.Hotel;

        _ = await _cityRepository.GetByIdAsync(hotelDto.CityId)
            ?? throw new NotFoundException("City", hotelDto.CityId);

        _ = await _hotelGroupRepository.GetByIdAsync(hotelDto.HotelGroupId)
            ?? throw new NotFoundException("Hotel group", hotelDto.HotelGroupId);

        if (hotelDto.DiscountId is Guid discountId)
        {
            _ = await _discountRepository.GetByIdAsync(discountId)
                ?? throw new NotFoundException("Discount", discountId);
        }
        
        var amenityIds = hotelDto.AmenityIds?
            .Distinct()
            .ToList() ?? new List<Guid>();

        if (amenityIds != null && amenityIds.Count > 0)
        {
            var existingAmenityIds = await _amenityRepository.Query()
                .Where(a => amenityIds.Contains(a.Id))
                .Select(a => a.Id)
                .ToListAsync(cancellationToken);

            var missing = amenityIds.Except(existingAmenityIds).ToList();
            if (missing.Count > 0)
            {
                throw new NotFoundException("Amenity", string.Join(", ", missing));
            }
        }

        var hotel = _mapper.Map<Hotel>(hotelDto);

        hotel.Id = Guid.NewGuid();
        hotel.CreatedAt = DateTime.UtcNow;

        await _hotelRepository.AddAsync(hotel);

        foreach (var amenityId in amenityIds!)
        {
            await _hotelAmenityRepository.AddAsync(new HotelAmenity
            {
                HotelId = hotel.Id,
                AmenityId = amenityId
            });
        }

        await _unitOfWork.SaveChangesAsync();
        return hotel.Id;
    }
}
