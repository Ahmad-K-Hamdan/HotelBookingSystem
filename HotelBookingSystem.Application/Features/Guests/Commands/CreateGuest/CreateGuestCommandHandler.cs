using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Guests;
using MediatR;

namespace HotelBookingSystem.Application.Features.Guests.Commands.CreateGuest;

public class CreateGuestCommandHandler : IRequestHandler<CreateGuestCommand, Guid>
{
    private readonly IGenericRepository<Guest> _guestRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGuestCommandHandler(IGenericRepository<Guest> guestRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateGuestCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated.");

        var existing = await _guestRepository.FindAsync(g => g.UserId == userId);

        if (existing.Any())
        {
            throw new IdentityException($"User '{userId}' is a registered guest.");
        }

        var guest = new Guest
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            PassportNumber = request.PassportNumber,
            HomeCountry = request.HomeCountry
        };

        await _guestRepository.AddAsync(guest);
        await _unitOfWork.SaveChangesAsync();
        return guest.Id;
    }
}
