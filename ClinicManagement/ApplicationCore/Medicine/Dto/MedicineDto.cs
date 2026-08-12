namespace ApplicationCore.Medicine.Dto;

public static class MedicineDto
{
    public record Request(
        string Name,
        string ActiveSubstance,
        string PharmaceuticalForm
    );

    public record Response(
        string Name,
        string ActiveSubstance,
        string PharmaceuticalForm);

    public record Details(
        string Name,
        string ActiveSubstance,
        string PharmaceuticalForm,
        string Dosage,
        string Frequency,
        string Instructions
    ) : Response(Name, ActiveSubstance, PharmaceuticalForm);
}