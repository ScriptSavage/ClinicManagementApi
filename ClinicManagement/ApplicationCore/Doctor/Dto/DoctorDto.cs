namespace ApplicationCore.Doctor.Dto;

public static class DoctorDto
{
    public record CreateDoctorDto(
        string Login,
        string Password,
        string Email,
        string FirstName,
        string LastName,
        string Pwz,
        IEnumerable<Guid> SpecializationsIds);
}