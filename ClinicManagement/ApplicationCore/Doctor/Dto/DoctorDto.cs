using ApplicationCore.Specialization.Dto;

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


    public record Response(
        string FirstName,
        string LastName,
        IEnumerable<SpecializationDto.NewSpecialization> Specializations
    );

    public record UpdateDoctorDto(
        string FirstName,
        string LastName,
        string Pwz
    );

    public record DoctorDetails(
        string FirstName,
        string LastName
    );
}