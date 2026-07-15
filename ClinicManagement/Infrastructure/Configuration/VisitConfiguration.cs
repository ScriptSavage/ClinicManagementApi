using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class VisitConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.HasKey(e => e.VisitId);
        
        builder.Property(e=>e.Date)
            .IsRequired();
        
        builder.HasOne(e=>e.Patient)
            .WithMany(e=>e.Visits)
            .HasForeignKey(e=>e.PatientId);
        
        builder.HasOne(e=>e.Doctor)
            .WithMany(e=>e.Visits)
            .HasForeignKey(e=>e.DoctorId);
    }
}