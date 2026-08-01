namespace ApplicationCore.Visit.Dto;

public class VisitDto
{
    public record Response(
        DateTime VisitDate,
        string? VisitDescription);
}