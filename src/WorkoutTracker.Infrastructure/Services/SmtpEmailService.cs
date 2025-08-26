namespace WorkoutTracker.Infrastructure;

using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using WorkoutTracker.Application.Shared.Models;
using WorkoutTracker.Application.Shared.Primitives;
using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Infrastructure.Models;

public sealed class SmtpEmailService(IOptions<SmtpEmailOptions> options) : IEmailService
{
    private readonly IOptions<SmtpEmailOptions> _options = options;

    public async Task<Result> SendEmailAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var client = new SmtpClient
            {
                Host = _options.Value.Host,
                Port = _options.Value.Port,
                EnableSsl = _options.Value.EnableSsl,
                Credentials = new NetworkCredential(
                    _options.Value.Username,
                    _options.Value.Password)
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(message.From.Value),
                Subject = message.Subject ?? string.Empty,
                Body = message.Body ?? string.Empty,
                IsBodyHtml = message.IsHtml
            };

            mailMessage.To.Add(new MailAddress(message.To.Value));

            await Task.Run(() => client.Send(mailMessage), cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Email.CannotSend);
        }
    }
}
