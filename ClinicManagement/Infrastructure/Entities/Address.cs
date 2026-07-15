namespace Infrastructure.Entities;

public class Address
{
    public Guid AddressId { get; set; }
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Street { get; set; } = null!;

    public Patient Patient { get; set; } = null!;
    public Guid PatientId { get; set; }
}