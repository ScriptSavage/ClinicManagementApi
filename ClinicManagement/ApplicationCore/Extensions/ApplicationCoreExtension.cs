using ApplicationCore.Auth.Dto;
using ApplicationCore.Auth.Services;
using ApplicationCore.Auth.Validators;
using ApplicationCore.Doctor.Services;
using ApplicationCore.Medicine.Services;
using ApplicationCore.Producer.Dto;
using ApplicationCore.Producer.Services;
using ApplicationCore.Producer.Validators;
using ApplicationCore.Specialization.Services;
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
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<ISpecializationService,SpecializationService>();
        services.AddScoped<IProducerService,ProducerService>();
        services.AddScoped<IMedicineService,MedicineService>();
        services.AddScoped<IValidator<AuthDto.RegisterNewPatient>, CreatePatientValidator>();
        services.AddScoped<IValidator<ProducerDto.NewProducer>, NewProducerValidator>();
    }
}