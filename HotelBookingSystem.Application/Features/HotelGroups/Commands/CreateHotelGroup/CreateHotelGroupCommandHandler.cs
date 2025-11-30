using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Hotels;
using MediatR;

namespace HotelBookingSystem.Application.Features.HotelGroups.Commands.CreateHotelGroup;

public class CreateHotelGroupCommandHandler : IRequestHandler<CreateHotelGroupCommand, Guid>
{
    private readonly IGenericRepository<HotelGroup> _hotelGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHotelGroupCommandHandler(
        IGenericRepository<HotelGroup> hotelGroupRepository,
        IUnitOfWork unitOfWork)
    {
        _hotelGroupRepository = hotelGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateHotelGroupCommand request, CancellationToken cancellationToken)
    {
        var exists = await _hotelGroupRepository.FindAsync(c => c.GroupName.ToLower() == request.GroupName.ToLower());

        if (exists.Count > 0)
        {
            throw new DuplicateRecordException("Exact hotel group already exists.");
        }

        var hotelGroup = new HotelGroup
        {
            Id = Guid.NewGuid(),
            GroupName = request.GroupName,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _hotelGroupRepository.AddAsync(hotelGroup);
        await _unitOfWork.SaveChangesAsync();
        return hotelGroup.Id;
    }
}
