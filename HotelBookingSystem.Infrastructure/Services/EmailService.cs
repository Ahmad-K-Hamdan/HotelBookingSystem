using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HotelBookingSystem.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly SendGridSettings _settings;
    private readonly ILogger<EmailService> _logger;
    private readonly ISendGridClient _sendGridClient;

    public EmailService(
        IOptions<SendGridSettings> settingsOptions,
        ILogger<EmailService> logger,
        ISendGridClient sendGridClient)
    {
        _settings = settingsOptions.Value;
        _logger = logger;
        _sendGridClient = sendGridClient;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        if (string.IsNullOrEmpty(_settings.ApiKey))
        {
            _logger.LogWarning("SendGrid API Key is NOT configured. Using the LoggingEmailService fallback if registered, otherwise email will not be sent to {ToEmail}.", to);
            return;
        }

        var msg = new SendGridMessage()
        {
            From = new EmailAddress(_settings.FromEmail, _settings.FromName),
            Subject = subject,
            HtmlContent = body
        };

        msg.AddTo(new EmailAddress(to));

        try
        {
            _logger.LogInformation("Attempting to send email to {ToEmail} from {FromEmail}...", to, _settings.FromEmail);

            var response = await _sendGridClient.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email successfully queued by SendGrid. Status: {StatusCode}", response.StatusCode);
                return;
            }
         
            var responseBody = await response.Body.ReadAsStringAsync();

            _logger.LogError("SendGrid API failed to process email to {ToEmail}. Status: {StatusCode}. Response Body: {Response}",
                to, response.StatusCode, responseBody);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "A connection or network exception occurred while sending email to {ToEmail}.", to);
        }
    }
}