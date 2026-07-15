namespace Infrastructure.Entities;

public class Visit
{
    public Guid VisitId { get; set; }
    
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;
    
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;


    public DateTime Date { get; set; }

    public DateTime CreateAt { get; set; } = DateTime.UtcNow;

    public string? Description { get; set; }
    public string? CancelReason { get; set; }
    public DateTime? CancelAt { get; set; }
    
}