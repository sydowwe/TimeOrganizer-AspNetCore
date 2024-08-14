namespace TimeOrganizer_net_core.security;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using System.Timers;
public interface IJwtService
{
    
}

public class JwtService : IHostedService, IDisposable
{
    private ECDsa _privateKey;
    private ECDsa _publicKey;
    private readonly List<string> _blacklist;
    private readonly Timer _blacklistCleanupTimer;
    private readonly ILogger<JwtService> _logger;

    public JwtService(ILogger<JwtService> logger)
    {
        _logger = logger;
        _blacklist = new List<string>();

        _blacklistCleanupTimer = new Timer
        {
            Interval = TimeSpan.FromSeconds(double.Parse(Environment.GetEnvironmentVariable("TOKEN_BLACKLIST_CLEANUP_PERIOD_IN_SEC"))).TotalMilliseconds,
            AutoReset = true
        };
        _blacklistCleanupTimer.Elapsed += (sender, e) => blacklistCleanUp();

        GenerateAndRefreshKeys();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _blacklistCleanupTimer.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _blacklistCleanupTimer.Stop();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _blacklistCleanupTimer?.Dispose();
    }

    private void GenerateAndRefreshKeys()
    {
        ECDSAKeyGenerator.generateAndSaveKeys();
        _privateKey = ECDSAKeyGenerator.readPrivateKey();
        _publicKey = ECDSAKeyGenerator.readPublicKey();
        _logger.LogInformation("ECDSA keys have been generated and loaded.");
    }

    public string extractEmail(string token)
    {
        return extractClaims(token, claims => claims.FindFirstValue(ClaimTypes.Email));
    }

    public long extractId(string token)
    {
        string id = extractClaims(token, claims => claims.FindFirstValue(JwtRegisteredClaimNames.Jti));
        if (long.TryParse(id, out long result))
        {
            return result;
        }
        throw new Exception($"Invalid ID in token: {id}");
    }

    public T extractClaims<T>(string token, Func<ClaimsPrincipal, T> claimsResolver)
    {
        var claimsPrincipal = extractAllClaims(token);
        return claimsResolver(claimsPrincipal);
    }

    private ClaimsPrincipal extractAllClaims(string token)
    {
        token = token.Substring(7); // Remove "Bearer " prefix
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new ECDsaSecurityKey(_publicKey),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };
        return tokenHandler.ValidateToken(token, validationParameters, out _);
    }

    public string generateToken(Dictionary<string, object> extraClaims, string email, long id, int expirationInHours)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, id.ToString())
        };

        foreach (var claim in extraClaims)
        {
            claims.Add(new Claim(claim.Key, claim.Value.ToString()));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(expirationInHours),
            SigningCredentials = new SigningCredentials(new ECDsaSecurityKey(_privateKey), SecurityAlgorithms.EcdsaSha384)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string generateToken(string email, long id, int expirationInHours)
    {
        return generateToken(new Dictionary<string, object>(), email, id, expirationInHours);
    }

    public void invalidateToken(string token)
    {
        _blacklist.Add(token.Substring(7)); // Remove "Bearer " prefix
    }

    public bool isTokenValid(string token, string username)
    {
        var email = extractEmail(token);
        return email == username && isTokenNotExpired(token) && !_blacklist.Contains(token);
    }

    private void blacklistCleanUp()
    {
        _blacklist.RemoveAll(token => !isTokenNotExpired(token));
        _logger.LogInformation("Blacklist cleanup performed.");
    }

    private bool isTokenNotExpired(string token)
    {
        return extractExpiration(token) > DateTime.UtcNow;
    }

    private DateTime extractExpiration(string token)
    {
        return extractClaims(token, claims => DateTime.Parse(claims.FindFirstValue(JwtRegisteredClaimNames.Exp)));
    }
}

