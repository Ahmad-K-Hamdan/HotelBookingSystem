using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingDetailsById;
using MediatR;

namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingInvoicePdf
{
    public class GetBookingInvoicePdfQueryHandler : IRequestHandler<GetBookingInvoicePdfQuery, byte[]>
    {
        private readonly IMediator _mediator;
        private readonly IBookingInvoicePdfGenerator _pdfGenerator;

        public GetBookingInvoicePdfQueryHandler(IMediator mediator, IBookingInvoicePdfGenerator pdfGenerator)
        {
            _mediator = mediator;
            _pdfGenerator = pdfGenerator;
        }

        public async Task<byte[]> Handle(GetBookingInvoicePdfQuery request, CancellationToken cancellationToken)
        {
            var bookingDetails = await _mediator.Send(new GetBookingDetailsByIdQuery(request.BookingId), cancellationToken);
            return _pdfGenerator.Generate(bookingDetails);
        }
    }
}
