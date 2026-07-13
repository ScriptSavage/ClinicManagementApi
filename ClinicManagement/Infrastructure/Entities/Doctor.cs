namespace Infrastructure.Entities;

public class Doctor
{
    public Guid DoctorId { get; set; }
    public string PWZ { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;


    public ApplicationUser User { get; set; } = null!;
    public Guid UserId { get; set; }

}