using Infrastructure.Configuration;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class DatabaseContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DatabaseContext()
    {
    }

    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ApplicationRole> Roles { get; set; } 
    public DbSet<Patient> Patients { get; set; } 


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ApplicationUserConfiguration());
        builder.ApplyConfiguration(new ApplicationRoleConfiguration());
        builder.ApplyConfiguration(new PatientConfiguration());
        builder.ApplyConfiguration(new DoctorConfiguration());
        base.OnModelCreating(builder);
        
    }
}