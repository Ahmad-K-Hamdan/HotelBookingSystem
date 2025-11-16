using HotelBookingSystem.Application.Common.Interfaces;

namespace HotelBookingSystem.Infrastructure.Services;

public class EmailService : IEmailService
{
    public Task SendEmailAsync(string to, string subject, string body)
    {
        throw new NotImplementedException();
    }
}