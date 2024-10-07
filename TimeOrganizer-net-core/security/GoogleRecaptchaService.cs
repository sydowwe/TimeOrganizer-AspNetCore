using System.Diagnostics.CodeAnalysis;
using TimeOrganizer_net_core.helper;

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
        var response = await httpClient.PostAsync($"{RecaptchaApiUrl}?secret={Helper.GetEnvVar("RECAPTCHA_SECRET")}&response={token}", null);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<RecaptchaResponse>(json);

        return result is { success: true } && result.score > 0.5 && expectedAction.Equals(result.action);
    }
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class RecaptchaResponse
{
    public bool success { get; init; }
    public float score { get; init; }
    public required string action { get; init; }
    public string? challengeTs { get; init; }
    public required string hostname { get; init; }
    public string[]? errorCodes { get; init; }
}
