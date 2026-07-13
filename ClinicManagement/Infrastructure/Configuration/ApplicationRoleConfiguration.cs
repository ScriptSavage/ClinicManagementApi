using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class ApplicationRoleConfiguration :  IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(e=>e.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasIndex(e=>e.Name)
            .IsUnique();
    }
}