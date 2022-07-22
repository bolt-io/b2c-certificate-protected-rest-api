using minimal_api_net6.Services;
using System.Security.Cryptography.X509Certificates;

namespace minimal_api_net6.Auth;

public sealed class CertificateValidationService : ICertificateValidationService
{
    private readonly ICertificateService _certificateService;

    public CertificateValidationService(ICertificateService certificateService)
    {
        _certificateService = certificateService ?? throw new ArgumentNullException(nameof(certificateService));
    }

    public async Task<bool> ValidateCertificateAsync(X509Certificate2 clientCertificate)
    {
        var expectedCertificate = await _certificateService.GetX509Certificate2Async("AuthCert");

        return clientCertificate.Thumbprint == expectedCertificate.Thumbprint;
    }
}
