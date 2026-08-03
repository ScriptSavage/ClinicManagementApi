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
}