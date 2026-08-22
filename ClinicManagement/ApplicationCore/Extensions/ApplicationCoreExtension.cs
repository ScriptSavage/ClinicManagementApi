using ApplicationCore.Address.Dto;
using ApplicationCore.Address.Validators;
using ApplicationCore.Auth.Dto;
using ApplicationCore.Auth.Services;
using ApplicationCore.Auth.Validators;
using ApplicationCore.Doctor.Dto;
using ApplicationCore.Doctor.Services;
using ApplicationCore.Doctor.Validators;
using ApplicationCore.Medicine.Services;
using ApplicationCore.Patient.Service;
using ApplicationCore.Prescription.Service;
using ApplicationCore.Producer.Dto;
using ApplicationCore.Producer.Services;
using ApplicationCore.Producer.Validators;
using ApplicationCore.Specialization.Services;
using ApplicationCore.Visit.Dto;
using ApplicationCore.Visit.Service;
using ApplicationCore.Visit.Validators;
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
        services.AddScoped<IPatientService,PatientService>();
        services.AddScoped<IVisitService,VisitService>();
        services.AddScoped<IPrescriptionService, PrescriptionService>();
        services.AddScoped<IValidator<AuthDto.RegisterNewPatient>, RegisterNewPatientValidator>();
        services.AddScoped<IValidator<AuthDto.LoginDto>, LoginValidator>();
        services.AddScoped<IValidator<AuthDto.ChangePasswordDto>, ChangePasswordValidator>();
        services.AddScoped<IValidator<DoctorDto.CreateDoctorDto>,CreateNewDoctorValidator>();
        services.AddScoped<IValidator<AddressDto.NewAddress>, NewAddressValidator>();
        services.AddScoped<IValidator<ProducerDto.NewProducer>, NewProducerValidator>();
        services.AddScoped<IValidator<VisitDto.CreateDescription>, CompleteVisitValidator>();
    }
}