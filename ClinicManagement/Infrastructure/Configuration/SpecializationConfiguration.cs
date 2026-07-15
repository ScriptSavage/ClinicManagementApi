using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
{
    public void Configure(EntityTypeBuilder<Specialization> builder)
    {
        builder.HasKey(k => k.SpecializationId);
        
        builder.Property(e=>e.Name)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasMany(e => e.Doctors)
            .WithMany(e => e.Specializations)
            .UsingEntity("DoctorSpecialization");
    }
}