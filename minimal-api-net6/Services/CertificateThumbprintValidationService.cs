using minimal_api_net6.Auth;
using System.Security.Cryptography.X509Certificates;

namespace minimal_api_net6.Services
{
    public class CertificateThumbprintValidationService : ICertificateValidationService
    {
        private readonly string[] validThumbprints = new[]
        {
            // add valid thumbprints here
            ""
        };

        public bool ValidateCertificate(X509Certificate2 clientCertificate)
            => validThumbprints.Contains(clientCertificate.Thumbprint);

        public Task<bool> ValidateCertificateAsync(X509Certificate2 clientCertificate)
        {
            return Task.FromResult(ValidateCertificate(clientCertificate));
        }
    }
}
