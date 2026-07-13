using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(e=>e.Email)
            .IsRequired()
            .HasMaxLength(250);
        
        builder.HasIndex(e=>e.Email)
            .IsUnique();
        
        builder.Property(e=>e.UserName)
            .IsRequired()
            .HasMaxLength(250);
        
        builder.HasIndex(e=>e.UserName)
            .IsUnique();
        
    }
}