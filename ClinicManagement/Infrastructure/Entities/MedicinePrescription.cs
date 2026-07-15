namespace Infrastructure.Entities;

public class MedicinePrescription
{
    public Medicine Medicine { get; set; } = null!;
    public Guid MedicineId { get; set; }

    public Prescription Prescription { get; set; } = null!;
    public Guid PrescriptionId { get; set; }


    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public string Quantity { get; set; } = null!;
    public string Instructions { get; set; } = null!;
    
    
}