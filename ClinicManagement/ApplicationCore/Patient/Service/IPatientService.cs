using ApplicationCore.Address.Dto;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Patient.Dto;
using ApplicationCore.Prescription.Dto;
using ApplicationCore.Visit.Dto;

namespace ApplicationCore.Patient.Service;

public interface IPatientService
{
    Task<PageResponse<PatientDto.PatientResponse>> GetAllPatientsAsync(int pageNumber, int pageSize);
    
    Task<PatientDto.PatientResponse> GetPatientByIdAsync(Guid id);
    
    Task UpdatePatientAsync(Guid patientId, PatientDto.UpdatePatientRequest request);
    
    Task DeletePatientAsync(Guid patientId);
    
    Task<PatientDto.PatientVisitsResponse> GetPatientVisitsAsync(string patientId);
    
    Task<IEnumerable<PatientDto.PatientPrescriptions>> GetPatientPrescriptionsAsync(string userId);
    
    Task<AddressDto.NewAddress> GetPatientAddressAsync(Guid patientId);
    
    Task UpdateMyData(string userId, PatientDto.UpdatePatientRequest request);
    
    Task<PageResponse<VisitDto.VisitDetailsResponse>> GetPatientVisitsAsync(Guid patientId, int page, int pageSize);
    
    Task<PageResponse<PrescriptionDto.PrescriptionDetails>> GetPatientPrescriptionsAsync(Guid patientId, int page,
        int pageSize);
}