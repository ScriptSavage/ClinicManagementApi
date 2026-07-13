namespace Infrastructure.Entities;

public class Patient
{
    public Guid PatientId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Pesel { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }


    public ApplicationUser ApplicationUser { get; set; } = null!;
    public Guid UserId { get; set; }
}