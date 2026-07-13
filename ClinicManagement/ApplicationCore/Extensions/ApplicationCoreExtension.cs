using ApplicationCore.Auth.Dto;
using ApplicationCore.Auth.Services;
using ApplicationCore.Auth.Validators;
using FluentValidation;
using Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore.Extensions;

public static class ApplicationCoreExtension
{
    public static void AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccessLayer(configuration);
        services.ConfigureAuth(configuration);

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IValidator<AuthDto.RegisterNewPatient>, CreatePatientValidator>();
    }
}