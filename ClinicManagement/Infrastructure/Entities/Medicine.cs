namespace Infrastructure.Entities;

public class Medicine
{
    public Guid MedicineId { get; set; }
    public string Name { get; set; } = null!;
    public string ActiveSubstance { get; set; } = null!;
    public string PharmaceuticalForm { get; set; } = null!;

    public ICollection<MedicinePrescription> MedicinePrescriptions { get; set; } = [];

    public Producer Producer { get; set; } = null!;
    public Guid ProducerId { get; set; }
}