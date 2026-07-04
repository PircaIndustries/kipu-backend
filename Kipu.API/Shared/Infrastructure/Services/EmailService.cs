using System.Net;
using System.Net.Mail;
using Kipu.API.Shared.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace Kipu.API.Shared.Infrastructure.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtpSettings = configuration.GetSection("SmtpSettings");
        
        using var client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"]))
        {
            Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
            EnableSsl = bool.Parse(smtpSettings["EnableSsl"])
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings["Username"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(to);

        await client.SendMailAsync(mailMessage);
    }
}
