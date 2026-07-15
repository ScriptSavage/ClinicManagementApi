using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.AddressId);
        
        builder.Property(e=>e.City)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(e => e.PostalCode)
            .IsRequired()
            .HasMaxLength(5);
        
        builder.Property(e => e.Street)
            .IsRequired()
            .HasMaxLength(150);
        
        
        builder.HasOne(e=>e.Patient)
            .WithOne(a => a.Address)
            .HasForeignKey<Address>(a => a.PatientId);
    }
}