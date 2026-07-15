using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
{
    public void Configure(EntityTypeBuilder<Producer> builder)
    {
        builder.HasKey(e => e.ProducerId);

        builder.HasIndex(e => e.Name)
            .IsUnique();
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(250);
    }
}