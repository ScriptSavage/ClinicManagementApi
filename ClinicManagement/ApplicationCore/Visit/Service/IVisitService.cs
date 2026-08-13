using ApplicationCore.Visit.Dto;

namespace ApplicationCore.Visit.Service;

public interface IVisitService
{
    Task<VisitDto.Response> CreateNewVisitAsync(string userId,VisitDto.CreateVisitRequest request);
}