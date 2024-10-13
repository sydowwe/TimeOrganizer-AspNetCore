namespace TimeOrganizer_net_core.helper;

using Microsoft.AspNetCore.Identity;
using model.entity;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public interface IMyEmailSender<T> : IEmailSender<T> where T: IdentityUser<long>
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}
public class EmailSender(IConfiguration configuration) : IMyEmailSender<User>
{
    private readonly string _smtpServer = Helper.GetEnvVar("MAIL_SMTP_SERVER");
    private readonly int _smtpPort = int.Parse(Helper.GetEnvVar("MAIL_SMTP_PORT"));
    private readonly string _smtpUsername = Helper.GetEnvVar("MAIL_SMTP_USERNAME");
    private readonly string _smtpPassword = Helper.GetEnvVar("MAIL_SMTP_PASSWORD");
    private readonly string _fromEmail = Helper.GetEnvVar("MAIL_FROM_EMAIL");
    private readonly string _appName = configuration.GetValue<string>("Application:Name") ?? throw new ArgumentNullException(nameof(configuration));
    private readonly string _appLogo = Helper.GetAppLogoUrl();
    private readonly string _templatePath = Path.Combine(Directory.GetCurrentDirectory(), "templates", "email");

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_fromEmail));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.SslOnConnect);
        await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        var template = await File.ReadAllTextAsync(Path.Combine(_templatePath, "ConfirmEmail.html"));
        var htmlContent = template
            .Replace("{{AppName}}", _appName)
            .Replace("{{AppLogoUrl}}", _appLogo)
            .Replace("{{Email}}", user.Email)
            .Replace("{{ConfirmationLink}}", confirmationLink)
            .Replace("{{CurrentYear}}", DateTime.Now.Year.ToString());

        await SendEmailAsync(email, $"Confirm your email for {_appName}", htmlContent);
    }


    public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        var template = await File.ReadAllTextAsync(Path.Combine(_templatePath, "ResetPassword.html"));
        var htmlContent = template
            .Replace("{{AppName}}", _appName)
            .Replace("{{AppLogoUrl}}", _appLogo)
            .Replace("{{Email}}", user.Email)
            .Replace("{{ResetLink}}", resetLink)
            .Replace("{{CurrentYear}}", DateTime.Now.Year.ToString());

        await SendEmailAsync(email, $"Reset your {_appName} password", htmlContent);
    }

    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        var template = await File.ReadAllTextAsync(Path.Combine(_templatePath, "ResetPasswordCode.html"));
        var htmlContent = template
            .Replace("{{AppName}}", _appName)
            .Replace("{{AppLogoUrl}}", _appLogo)
            .Replace("{{Email}}", user.Email)
            .Replace("{{ResetCode}}", resetCode)
            .Replace("{{CurrentYear}}", DateTime.Now.Year.ToString());

        await SendEmailAsync(email, $"Your {_appName} password reset code", htmlContent);
    }
}