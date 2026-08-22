using ApplicationCore.Medicine.Dto;
using ApplicationCore.Patient.Dto;

namespace ApplicationCore.Prescription.Dto;

public class PrescriptionDto
{
    public record Response(
        DateTime Date,
        string Code);

    public record Request(
        IReadOnlyCollection<MedicineItemRequest> Medicines
    );

    public record MedicineItemRequest(
        Guid MedicineId,
        string Dosage,
        string Frequency,
        string Quantity,
        string Instructions
    );

    public record Details(
        string Frequency,
        string Quantity,
        string Instructions);

    public record PrescriptionDetails(
        PatientDto.Response Patient,
        DateTime Date,
        string Code,
        IEnumerable<MedicineDto.Details> MedicineDetails
    );
}