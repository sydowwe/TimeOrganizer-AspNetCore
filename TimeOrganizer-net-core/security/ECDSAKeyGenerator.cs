using System.Text;

namespace TimeOrganizer_net_core.security;

using System;
using System.IO;
using System.Security.Cryptography;

public static class ECDSAKeyGenerator
{
    private const string PrivateKeyFilePath = "keys/ec-private-key.pem";
    private const string PublicKeyFilePath = "keys/ec-public-key.pem";
    private static readonly string PemPassword = Environment.GetEnvironmentVariable("PEM_PASSWORD") ?? throw new Exception("NO PEM_PASSWORD Env Variable");
    
    public static ECDsa readPrivateKey()
    {
        var privateKeyPem = File.ReadAllText(PrivateKeyFilePath);
        var ecdsa = ECDsa.Create();
        ecdsa.ImportFromEncryptedPem(privateKeyPem, PemPassword);
        return ecdsa;
    }

    public static ECDsa readPublicKey()
    {
        var publicKeyPem = File.ReadAllBytes(PublicKeyFilePath);
        var ecdsa = ECDsa.Create();
        ecdsa.ImportSubjectPublicKeyInfo(publicKeyPem,out _);
        return ecdsa;
    }
    public static void generateAndSaveKeys()
    {
        using (var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP384))
        {
            createDirectoriesIfNotExist();
            var privateKeyPem = ecdsa.ExportEncryptedPkcs8PrivateKeyPem(Encoding.UTF8.GetBytes(PemPassword).AsSpan(),
                new PbeParameters(PbeEncryptionAlgorithm.Aes256Cbc,HashAlgorithmName.SHA256,10000));
            File.WriteAllText(PrivateKeyFilePath, privateKeyPem);

            var publicKeyPem = ecdsa.ExportSubjectPublicKeyInfoPem();
            File.WriteAllText(PublicKeyFilePath, publicKeyPem);

            Console.WriteLine("Keys generated and saved.");
        }
    }
    private static void createDirectoriesIfNotExist()
    {
        var privateKeyParentDirectory = new FileInfo(PrivateKeyFilePath).Directory;
        var publicKeyParentDirectory = new FileInfo(PublicKeyFilePath).Directory;
        if (privateKeyParentDirectory is { Exists: false })
        {
            privateKeyParentDirectory.Create();
        }
        if (publicKeyParentDirectory is { Exists: false })
        {
            publicKeyParentDirectory.Create();
        }
    }
}