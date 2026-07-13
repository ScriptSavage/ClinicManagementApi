using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(e => e.DoctorId);
        
        
        builder.Property(e=>e.FirstName)
            .IsRequired()
            .HasMaxLength(250);
        
        builder.Property(e=>e.LastName)
            .IsRequired()
            .HasMaxLength(250);
        
        builder.Property(e=>e.PWZ)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.HasIndex(e=>e.PWZ)
            .IsUnique();
        
        
        builder.HasOne(e=>e.User)
            .WithOne(e => e.Doctor)
            .HasForeignKey<Doctor>(e => e.UserId);
        
    }
}