namespace ApplicationCore.Specialization.Dto;

public static class SpecializationDto
{
    public record AddNewSpecialization(
        string SpecializationName,
        string SpecializationDescription);
}