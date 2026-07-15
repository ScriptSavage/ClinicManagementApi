using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class MedicinePrescriptionConfiguration : IEntityTypeConfiguration<MedicinePrescription>
{
    public void Configure(EntityTypeBuilder<MedicinePrescription> builder)
    {
        builder.HasKey(e => new { e.PrescriptionId, e.MedicineId });

        builder.Property(e => e.Dosage)
            .IsRequired();
        
        builder.Property(e => e.Frequency)
            .IsRequired();
        
        builder.Property(e => e.Quantity)
            .IsRequired();

        builder.Property(e => e.Instructions)
            .IsRequired();

        builder.HasOne(e => e.Prescription)
            .WithMany(e => e.MedicinePrescriptions)
            .HasForeignKey(e => e.PrescriptionId);
        
        builder.HasOne(e => e.Medicine)
            .WithMany(e => e.MedicinePrescriptions)
            .HasForeignKey(e => e.MedicineId);

    }
}