using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Hotels;
using MediatR;

namespace HotelBookingSystem.Application.Features.Hotels.Commands.DeleteHotel;

public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, Unit>
{
    private readonly IGenericRepository<Hotel> _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHotelCommandHandler(IGenericRepository<Hotel> hotelRepository, IUnitOfWork unitOfWork)
    {
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.Id);

        if (hotel == null)
        {
            throw new NotFoundException(nameof(hotel), request.Id);
        }

        _hotelRepository.Delete(hotel);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
