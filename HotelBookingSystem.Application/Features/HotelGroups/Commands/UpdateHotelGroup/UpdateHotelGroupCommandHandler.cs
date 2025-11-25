using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Hotels;
using MediatR;

namespace HotelBookingSystem.Application.Features.HotelGroups.Commands.UpdateHotelGroup;

public class UpdateHotelGroupCommandHandler : IRequestHandler<UpdateHotelGroupCommand, Unit>
{
    private readonly IGenericRepository<HotelGroup> _hotelGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHotelGroupCommandHandler(IGenericRepository<HotelGroup> hotelGroupRepository, IUnitOfWork unitOfWork)
    {
        _hotelGroupRepository = hotelGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateHotelGroupCommand request, CancellationToken cancellationToken)
    {
        var hotelGroup = await _hotelGroupRepository.GetByIdAsync(request.Id);

        if (hotelGroup == null)
        {
            throw new NotFoundException(nameof(hotelGroup), request.Id);
        }

        hotelGroup.GroupName = request.GroupName;
        hotelGroup.Description = request.Description;
        hotelGroup.UpdatedAt = DateTime.UtcNow;

        _hotelGroupRepository.Update(hotelGroup);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}