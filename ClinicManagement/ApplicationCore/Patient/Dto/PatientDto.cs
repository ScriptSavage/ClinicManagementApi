using ApplicationCore.Address.Dto;

namespace ApplicationCore.Patient.Dto;

public static class PatientDto
{
    public record PatientResponse(
        string FirstName,
        string LastName,
        string Pesel,
        DateTime DateOfBirth,
        AddressDto.NewAddress Address
    );
}