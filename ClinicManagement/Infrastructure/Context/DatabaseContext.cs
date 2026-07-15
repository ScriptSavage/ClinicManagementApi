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
    public DbSet<Specialization> Specializations { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Patient> Patients { get; set; } 
    public DbSet<Visit> Visits { get; set; } 
    public DbSet<Prescription> Prescriptions { get; set; } 
    public DbSet<Producer> Producers { get; set; } 
    public DbSet<Medicine> Medicines { get; set; } 
    public DbSet<MedicinePrescription> MedicinePrescription { get; set; } 


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ApplicationUserConfiguration());
        builder.ApplyConfiguration(new ApplicationRoleConfiguration());
        builder.ApplyConfiguration(new PatientConfiguration());
        builder.ApplyConfiguration(new DoctorConfiguration());
        builder.ApplyConfiguration(new AddressConfiguration());
        builder.ApplyConfiguration(new SpecializationConfiguration());
        builder.ApplyConfiguration(new VisitConfiguration());
        builder.ApplyConfiguration(new PrescriptionConfiguration());
        builder.ApplyConfiguration(new ProducerConfiguration());
        builder.ApplyConfiguration(new MedicineConfiguration());
        builder.ApplyConfiguration(new MedicinePrescriptionConfiguration());
        base.OnModelCreating(builder);
        
    }
}