# b2c-certificate-protected-rest-api
Calling a certificate protected rest api with Azure AD B2C custom policies


# minimal-api-net6
In the web app:

Add a reference to the [Microsoft.AspNetCore.Authentication.Certificate](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.Certificate) NuGet package.
In Program.cs, call builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate(...);. Provide a delegate for OnCertificateValidated to do any supplementary validation on the client certificate sent with requests. Turn that information into a ClaimsPrincipal and set it on the context.Principal property.