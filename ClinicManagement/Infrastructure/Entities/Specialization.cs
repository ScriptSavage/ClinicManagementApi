namespace Infrastructure.Entities;

public class Specialization
{
    public Guid SpecializationId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ICollection<Doctor> Doctors { get; set; } = [];
}