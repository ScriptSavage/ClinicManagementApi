using ApplicationCore.Address.Dto;
using ApplicationCore.Visit.Dto;

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

    public record UpdatePatientRequest(
        string FirstName,
        string LastName,
        AddressDto.NewAddress Address
    );

    public record PatientVisitsResponse(
        string FirstName,
        string LastName,
        IEnumerable<VisitDto.Response> Visits
    );
}