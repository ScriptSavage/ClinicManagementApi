using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore.Seeders;

public static class AdminSeeder
{
    public static async Task SeedAdminAsync(IServiceProvider services, IConfiguration configuration)
    {
        using var scope = services.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        
        
        var adminConfiguration =  configuration.GetSection("AdminUser");
        
        
        var adminEmail = adminConfiguration["Email"];
        var adminPassword = adminConfiguration["Password"];
        var adminUserName = adminConfiguration["Username"];

        if (string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminPassword) ||
            string.IsNullOrWhiteSpace(adminUserName))
        {
            return;
        }
        
        
        var existingAdmin = await userManager.FindByNameAsync(adminUserName);

        if (existingAdmin is null)
        {
            var newAdmin = new ApplicationUser
            {
                UserName = adminUserName,
                Email = adminEmail
            };

            await userManager.CreateAsync(newAdmin, adminPassword);


            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new ApplicationRole{Name = "Admin"});
            }


            if (!await userManager.IsInRoleAsync(newAdmin, "Admin"))
            {
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }
        
    }
}
