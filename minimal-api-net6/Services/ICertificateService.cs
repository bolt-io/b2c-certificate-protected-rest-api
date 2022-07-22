using System.Security.Cryptography.X509Certificates;

namespace minimal_api_net6.Services
{
    public interface ICertificateService
    {
        Task<X509Certificate2> GetX509Certificate2Async(string certificateName);
    }
}