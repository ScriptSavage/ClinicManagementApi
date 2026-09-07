using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.RefreshTokenId);
        builder.Property(x => x.TokenHash).IsRequired().HasMaxLength(64).IsUnicode(false);
        builder.Property(x => x.ReplacedByTokenHash).HasMaxLength(64).IsUnicode(false);
        builder.Property(x => x.SecurityStamp).IsRequired();
        builder.HasIndex(x => x.TokenHash).IsUnique();
        builder.HasIndex(x => x.FamilyId);
        builder.Ignore(x => x.IsActive);

        builder.HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
