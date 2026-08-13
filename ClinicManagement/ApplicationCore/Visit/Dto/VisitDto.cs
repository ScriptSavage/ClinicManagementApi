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
}