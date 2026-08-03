namespace ApplicationCore.Prescription.Dto;

public class PrescriptionDto
{
    public record Response(
        DateTime Date,
        string Code);

    public record Details(
        string Frequency,
        string Quantity,
        string Instructions);
}