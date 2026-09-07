namespace Infrastructure.Entities;

public class RefreshToken
{
    public Guid RefreshTokenId { get; set; }
    
    public string TokenHash { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public Guid FamilyId { get; set; }
    public string SecurityStamp { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime Expires { get; set; }
    public DateTime? Revoked { get; set; }

    public string? ReplacedByTokenHash { get; set; }
    public bool IsActive => Revoked is null && DateTime.UtcNow < Expires;
}
