namespace TimeOrganizer_net_core.helper;

using Microsoft.AspNetCore.Identity;
using model.entity;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public class EmailSender : IEmailSender<User>
{
    private readonly string _smtpServer = Helper.GetEnvVar("EmailSender_SMTP_Server");
    private readonly int _smtpPort = int.Parse(Helper.GetEnvVar("EmailSender_SMTP_Port"));
    private readonly string _smtpUsername = Helper.GetEnvVar("EmailSender_SMTP_Username");
    private readonly string _smtpPassword = Helper.GetEnvVar("EmailSender_SMTP_Password");
    private readonly string _myEmail = Helper.GetEnvVar("EmailSender_MyEmail");
    private readonly string _appName = Helper.GetEnvVar("AppName");
    private readonly string _appLogo = Helper.GetEnvVar("AppLogo");
    private readonly string _templatePath = Path.Combine(Directory.GetCurrentDirectory(), "templates", "email");

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_myEmail));
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
            .Replace("{{CompanyName}}", _appName)
            .Replace("{{CompanyLogo}}", _appLogo)
            .Replace("{{Email}}", user.Email)
            .Replace("{{ConfirmationLink}}", confirmationLink)
            .Replace("{{CurrentYear}}", DateTime.Now.Year.ToString());

        await SendEmailAsync(email, $"Confirm your email for {_appName}", htmlContent);
    }


    public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        var template = await File.ReadAllTextAsync(Path.Combine(_templatePath, "ResetPassword.html"));
        var htmlContent = template
            .Replace("{{CompanyName}}", _appName)
            .Replace("{{CompanyLogo}}", _appLogo)
            .Replace("{{Email}}", user.Email)
            .Replace("{{ResetLink}}", resetLink)
            .Replace("{{CurrentYear}}", DateTime.Now.Year.ToString());

        await SendEmailAsync(email, $"Reset your {_appName} password", htmlContent);
    }

    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        var template = await File.ReadAllTextAsync(Path.Combine(_templatePath, "ResetPasswordCode.html"));
        var htmlContent = template
            .Replace("{{CompanyName}}", _appName)
            .Replace("{{CompanyLogo}}", _appLogo)
            .Replace("{{Email}}", user.Email)
            .Replace("{{ResetCode}}", resetCode)
            .Replace("{{CurrentYear}}", DateTime.Now.Year.ToString());

        await SendEmailAsync(email, $"Your {_appName} password reset code", htmlContent);
    }
}