using OtpNet;
using QRCoder;

namespace TimeOrganizer_net_core.security;

public interface ITOTPService
{
    string generateSecretKey();
    byte[] generateQrCode(string secretKey, string userEmail);
    bool validateTotp(string secretKey, string code);
}

public class TOTPService : ITOTPService
{
    public string generateSecretKey()
    {
        // Generate a random key for TOTP
        var key = KeyGeneration.GenerateRandomKey(20);
        return Base32Encoding.ToString(key);
    }
    public byte[] generateQrCode(string secretKey, string userEmail)
    {
        var totp = new Totp(Base32Encoding.ToBytes(secretKey));
        var totpUrl = totp.ToString();
        var appName = Environment.GetEnvironmentVariable("APP_NAME") ?? throw new Exception("No environment variable APP_NAME");
        var otpAuthUrl = $"otpauth://totp/{appName}:{userEmail}?secret={secretKey}&issuer={appName}&digits=6";
        
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(otpAuthUrl, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }
    public bool validateTotp(string secretKey, string code)
    {
        var totp = new Totp(Base32Encoding.ToBytes(secretKey));
        return totp.VerifyTotp(code, out long timeWindowUsed, new VerificationWindow(previous: 1, future: 1));
    }
}