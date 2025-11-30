using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Hotels;
using MediatR;

namespace HotelBookingSystem.Application.Features.HotelImages.Commands.DeleteHotelImage;

public class DeleteHotelImageCommandHandler : IRequestHandler<DeleteHotelImageCommand, Unit>
{
    private readonly IGenericRepository<HotelImage> _imageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHotelImageCommandHandler(
        IGenericRepository<HotelImage> imageRepository,
        IUnitOfWork unitOfWork)
    {
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteHotelImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _imageRepository.GetByIdAsync(request.Id);

        if (image is null)
        {
            throw new NotFoundException(nameof(HotelImage), request.Id);
        }

        _imageRepository.Delete(image);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
