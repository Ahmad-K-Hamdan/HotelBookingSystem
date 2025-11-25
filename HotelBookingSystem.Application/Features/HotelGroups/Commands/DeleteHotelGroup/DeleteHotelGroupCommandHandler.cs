using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Hotels;
using MediatR;

namespace HotelBookingSystem.Application.Features.HotelGroups.Commands.DeleteHotelGroup;

public class DeleteHotelGroupCommandHandler : IRequestHandler<DeleteHotelGroupCommand, Unit>
{
    private readonly IGenericRepository<HotelGroup> _hotelGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHotelGroupCommandHandler(IGenericRepository<HotelGroup> hotelGroupRepository, IUnitOfWork unitOfWork)
    {
        _hotelGroupRepository = hotelGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteHotelGroupCommand request, CancellationToken cancellationToken)
    {
        var hotelGroup = await _hotelGroupRepository.GetByIdAsync(request.Id);

        if (hotelGroup == null)
        {
            throw new NotFoundException(nameof(hotelGroup), request.Id);
        }

        _hotelGroupRepository.Delete(hotelGroup);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}