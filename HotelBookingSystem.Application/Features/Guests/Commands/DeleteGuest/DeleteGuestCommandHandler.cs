using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Guests;
using MediatR;

namespace HotelBookingSystem.Application.Features.Guests.Commands.DeleteGuest;

public class DeleteGuestCommandHandler : IRequestHandler<DeleteGuestCommand, Unit>
{
    private readonly IGenericRepository<Guest> _guestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGuestCommandHandler(IGenericRepository<Guest> guestRepository, IUnitOfWork unitOfWork)
    {
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteGuestCommand request, CancellationToken cancellationToken)
    {
        var guest = await _guestRepository.GetByIdAsync(request.Id);

        if (guest == null)
        {
            throw new NotFoundException(nameof(guest), request.Id);
        }

        _guestRepository.Delete(guest);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
