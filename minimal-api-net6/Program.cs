using Azure.Identity;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using minimal_api_net6.Auth;
using minimal_api_net6.Services;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.ConfigureHttpsDefaults(options =>
        options.ClientCertificateMode = ClientCertificateMode.RequireCertificate);
});

// uncomment the two lines below to use key vault for certificate validation.
//builder.Services.AddSingleton<ICertificateService, KeyVaultCertificateService>(sp => new(builder.Configuration["KeyVaultName"]));
//builder.Services.AddSingleton<ICertificateValidationService, CertificateValidationService>();

// comment out this line if using key vault for certificate validation.
builder.Services.AddSingleton<ICertificateValidationService, CertificateThumbprintValidationService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(options =>
    {
        // change the allowed certificate types to suit your needs (chained or self-signed)
        options.AllowedCertificateTypes = CertificateTypes.All;
        options.Events = new CertificateAuthenticationEvents()
        {
            OnCertificateValidated = async context =>
            {
                var validationService = context.HttpContext.RequestServices.GetRequiredService<ICertificateValidationService>();

                if (!await validationService.ValidateCertificateAsync(context.ClientCertificate))
                {
                    context.Fail("Invalid certificate");
                    return;
                }

                var claims = new[]
{
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        context.ClientCertificate.Subject,
                        ClaimValueTypes.String, context.Options.ClaimsIssuer),
                    new Claim(
                        ClaimTypes.Name,
                        context.ClientCertificate.Subject,
                        ClaimValueTypes.String, context.Options.ClaimsIssuer)
                };

                context.Principal = new ClaimsPrincipal(
                    new ClaimsIdentity(claims, context.Scheme.Name));
                context.Success();
            }
        };
    })
    .AddCertificateCache(options =>
    {
        options.CacheSize = 1024;
        options.CacheEntryExpiration = TimeSpan.FromMinutes(2);
    });



var app = builder.Build();

app.UseAuthentication();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("api/users/{objectId}", (Guid objectId) =>
{
    var objectIdStart = objectId.ToString()[..8];
    var objectIdEnd = objectId.ToString()[20..];


    UserProfile userProfile = new(objectIdStart, objectIdEnd);
    return userProfile;
})
.WithName("GetUserProfile");

app.Run();

internal record UserProfile(string Givenname, string SurName);

