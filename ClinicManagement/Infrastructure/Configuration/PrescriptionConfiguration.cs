using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.HasKey(e=>e.PrescriptionId);

        builder.HasOne(e => e.Patient)
            .WithMany(p => p.Prescriptions)
            .HasForeignKey(e => e.PatientId);
        
        builder.HasOne(e => e.Doctor)
            .WithMany(p => p.Prescriptions)
            .HasForeignKey(e => e.DoctorId);
        
    }
}