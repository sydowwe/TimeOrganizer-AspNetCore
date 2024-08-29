namespace TimeOrganizer_net_core.security;

using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public interface IGoogleRecaptchaService
{
    Task<bool> VerifyRecaptchaAsync(string token, string expectedAction);
}

public class GoogleRecaptchaService(HttpClient httpClient) : IGoogleRecaptchaService
{
    private const string RecaptchaApiUrl = "https://www.google.com/recaptcha/api/siteverify";

    public async Task<bool> VerifyRecaptchaAsync(string token, string expectedAction)
    {
        var response = await httpClient.PostAsync($"{RecaptchaApiUrl}?secret={Environment.GetEnvironmentVariable("RECAPTCHA_SECRET")}&response={token}", null);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<RecaptchaResponse>(json);

        return result is { Success: true } && result.Score > 0.5 && expectedAction.Equals(result.Action);
    }
}

public class RecaptchaResponse
{
    public bool Success { get; init; }
    public float Score { get; init; }
    public string Action { get; init; }
    public string ChallengeTs { get; init; }
    public string Hostname { get; init; }
    public string[] ErrorCodes { get; init; }
}
