using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastUpdatedAt { get; set; } = DateTime.UtcNow;


    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }

}