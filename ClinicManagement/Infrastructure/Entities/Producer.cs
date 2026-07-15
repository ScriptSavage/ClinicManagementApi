namespace Infrastructure.Entities;

public class Producer
{
    public Guid ProducerId { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Medicine> Medicines { get; set; } = [];
}