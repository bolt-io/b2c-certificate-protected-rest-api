using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using System.Security.Cryptography.X509Certificates;

namespace minimal_api_net6.Services;

public sealed class KeyVaultCertificateService : ICertificateService
{

    private readonly CertificateClient _certificateClient;


    public KeyVaultCertificateService(string keyVaultName)
    {
        if (string.IsNullOrWhiteSpace(keyVaultName))
        {
            throw new ArgumentException($"'{nameof(keyVaultName)}' cannot be null or whitespace.", nameof(keyVaultName));
        }


        _certificateClient = new CertificateClient(vaultUri: new Uri($"https://{keyVaultName}.vault.azure.net/"),
                                                   credential: new DefaultAzureCredential());
    }


    public async Task<X509Certificate2> GetX509Certificate2Async(string certificateName)
    {
        if (string.IsNullOrWhiteSpace(certificateName))
        {
            throw new ArgumentException($"'{nameof(certificateName)}' cannot be null or whitespace.", nameof(certificateName));
        }
        
        var cert = await _certificateClient.DownloadCertificateAsync(certificateName).ConfigureAwait(false);
        return cert.Value;
    }
}
