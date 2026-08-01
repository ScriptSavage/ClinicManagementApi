namespace ApplicationCore.Specialization.Dto;

public static class SpecializationDto
{
    public record NewSpecialization(
        string SpecializationName,
        string SpecializationDescription);

    public record Response(string SpecializationName, string SpecializationDescription) : 
        NewSpecialization(SpecializationName, SpecializationDescription)
    {
        
    }
}