using Infrastructure.Context;
using Infrastructure.Helpers;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class InfrastructureExtension
{
    public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        

        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });


        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();


    }
}