namespace Infrastructure.Entities;

public class Prescription
{
    public Guid PrescriptionId { get; set; }
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;
    
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public ICollection<MedicinePrescription> MedicinePrescriptions { get; set; } = [];

    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;

    public string Code { get; set; } = null!;

}