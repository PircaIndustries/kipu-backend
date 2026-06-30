namespace Kipu.API.Shared.Domain.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}
