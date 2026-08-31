using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Visit.Dto;

namespace ApplicationCore.Visit.Service;

public interface IVisitService
{
    Task<VisitDto.Response> CreateNewVisitAsync(string userId,VisitDto.CreateVisitRequest request);
    
    Task<VisitDto.VisitDetailsResponse> GetVisitDetailsAsync(Guid visitId);
    
    Task<IEnumerable<VisitDto.VisitDetailsResponse>> GetMyVisitsAsync(string userId);
    Task<PageResponse<VisitDto.VisitDetailsResponse>> GetVisitsDetailsAsync(int page, int pageSize);
    
    Task CompleteVisitAsync(Guid visitId, string userId, VisitDto.CreateDescription dto);
    
    Task RescheduleVisitAsync(string userId, Guid visitId, VisitDto.Reschedule reschedule);
    Task CancelVisitAsync(string userId, Guid visitId);
    
}