using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SmartStay.Application.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    private readonly string _host = configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
    private readonly int _port = int.TryParse(configuration["Email:SmtpPort"], out var p) ? p : 587;
    private readonly string _user = configuration["Email:SmtpUser"] ?? string.Empty;
    private readonly string _pass = configuration["Email:SmtpPass"] ?? string.Empty;
    private readonly string _fromName = configuration["Email:FromName"] ?? "SmartStay";

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_fromName, _user));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart(isHtml ? MimeKit.Text.TextFormat.Html : MimeKit.Text.TextFormat.Plain)
        {
            Text = body
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_user, _pass);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
