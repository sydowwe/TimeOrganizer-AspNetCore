namespace TimeOrganizer_net_core.helper;

public interface IEmailService
{
    string GenerateForgottenPasswordEmail(string token);
    Task SendEmailAsync(string email, string subject, string emailBody);
}

public class EmailService : IEmailService
{
    public string GenerateForgottenPasswordEmail(string token)
    {
        throw new NotImplementedException();
    }

    public Task SendEmailAsync(string email, string subject, string emailBody)
    {
        throw new NotImplementedException();
    }
}

