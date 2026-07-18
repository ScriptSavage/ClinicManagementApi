using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore.Seeders;

public static class RoleSeeder
{
    private static string[] _roles =
    {
        "Admin",
        "Patient",
        "Doctor",
        "Employee"
    };


    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        foreach (var roleName in _roles)
        {
            if (await roleManager.RoleExistsAsync(roleName)) continue;

            await roleManager.CreateAsync(new ApplicationRole
            {
                Name = roleName
            });
            
        }
    }

}