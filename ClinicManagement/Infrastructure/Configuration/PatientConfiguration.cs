using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(e => e.PatientId);
        
        builder.Property(e=>e.FirstName)
            .HasMaxLength(250)
            .IsRequired();
        
        builder.Property(e=>e.LastName)
            .HasMaxLength(250)
            .IsRequired();
        
        builder.Property(e=>e.Pesel)
            .HasMaxLength(11)
            .IsRequired();
        
        builder.HasIndex(e=>e.Pesel)
            .IsUnique();
        
        builder.Property(e=>e.DateOfBirth)
            .IsRequired();

        builder.HasOne(e => e.ApplicationUser)
            .WithOne(e => e.Patient)
            .HasForeignKey<Patient>(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(e=>e.UserId)
            .IsUnique();

    }
}