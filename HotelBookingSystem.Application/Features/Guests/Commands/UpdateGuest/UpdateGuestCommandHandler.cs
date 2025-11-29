using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Guests;
using MediatR;

namespace HotelBookingSystem.Application.Features.Guests.Commands.UpdateGuest;

public class UpdateGuestCommandHandler : IRequestHandler<UpdateGuestCommand, Unit>
{
    private readonly IGenericRepository<Guest> _guestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGuestCommandHandler(IGenericRepository<Guest> guestRepository, IUnitOfWork unitOfWork)
    {
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateGuestCommand request, CancellationToken cancellationToken)
    {
        var guest = await _guestRepository.GetByIdAsync(request.Id);

        if (guest == null)
        {
            throw new NotFoundException(nameof(guest), request.Id);
        }

        guest.PassportNumber = request.PassportNumber;
        guest.HomeCountry = request.HomeCountry;

        _guestRepository.Update(guest);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
