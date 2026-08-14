using ApplicationCore.Doctor.Dto;
using ApplicationCore.Patient.Dto;

namespace ApplicationCore.Visit.Dto;

public class VisitDto
{
    public record Response(
        DateTime VisitDate,
        string? VisitDescription);


    public record CreateVisitRequest(
        Guid DoctorId,
        DateTime VisitDate
    );

    public record VisitDetailsResponse(
        DoctorDto.UpdateDoctorDto Doctor,
        PatientDto.Response Patient,
        DateTime VisitDate,
        string? VisitDescription
    );
}