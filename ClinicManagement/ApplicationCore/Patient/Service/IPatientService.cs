using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Patient.Dto;

namespace ApplicationCore.Patient.Service;

public interface IPatientService
{
    Task<PageResponse<PatientDto.PatientResponse>> GetAllPatientsAsync(int pageNumber, int pageSize);
}