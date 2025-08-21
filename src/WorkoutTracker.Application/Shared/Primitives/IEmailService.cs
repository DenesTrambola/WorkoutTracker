namespace WorkoutTracker.Application.Shared.Primitives;

using WorkoutTracker.Application.Shared.Models;
using WorkoutTracker.Domain.Shared.Results;

public interface IEmailService
{
    Task<Result> SendEmailAsync(EmailMessage message,
        CancellationToken cancellationToken = default);

    Task<Result> SendBulkEmailAsync(IEnumerable<EmailMessage> messages,
        CancellationToken cancellationToken = default);
}
