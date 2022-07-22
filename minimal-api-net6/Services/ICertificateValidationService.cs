using System.Security.Cryptography.X509Certificates;

namespace minimal_api_net6.Auth;

public interface ICertificateValidationService
{
    Task<bool> ValidateCertificateAsync(X509Certificate2 clientCertificate);
}