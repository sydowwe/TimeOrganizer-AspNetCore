namespace TimeOrganizer_net_core.security;

using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public interface IGoogleRecaptchaService
{
    Task<bool> verifyRecaptchaAsync(string token, string expectedAction);
}

public class GoogleRecaptchaService(HttpClient httpClient) : IGoogleRecaptchaService
{
    private const string RecaptchaApiUrl = "https://www.google.com/recaptcha/api/siteverify";

    public async Task<bool> verifyRecaptchaAsync(string token, string expectedAction)
    {
        var response = await httpClient.PostAsync($"{RecaptchaApiUrl}?secret={Environment.GetEnvironmentVariable("RECAPTCHA_SECRET")}&response={token}", null);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<RecaptchaResponse>(json);

        return result.success && result.score > 0.5 && expectedAction.Equals(result.action);
    }
}

public class RecaptchaResponse
{
    public bool success { get; init; }
    public float score { get; init; }
    public string action { get; init; }
    public string challengeTs { get; init; }
    public string hostname { get; init; }
    public string[] errorCodes { get; init; }
}
