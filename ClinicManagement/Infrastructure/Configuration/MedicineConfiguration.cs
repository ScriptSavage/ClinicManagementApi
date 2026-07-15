using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class MedicineConfiguration : IEntityTypeConfiguration<Medicine>
{
    public void Configure(EntityTypeBuilder<Medicine> builder)
    {
        builder.HasKey(m => m.MedicineId);
        
        builder.Property(e=>e.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(e=>e.PharmaceuticalForm)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(e=>e.ActiveSubstance)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(e => e.Producer)
            .WithMany(m => m.Medicines)
            .HasForeignKey(e => e.ProducerId);
    }
}