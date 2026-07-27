using ApplicationCore.Address.Dto;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Patient.Dto;
using Infrastructure.Repositories.Patient;

namespace ApplicationCore.Patient.Service;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<PageResponse<PatientDto.PatientResponse>> GetAllPatientsAsync(int pageNumber, int pageSize)
    {
        var patients = (await _patientRepository.GetAllPatientsAsync()).AsQueryable();

        var data = patients.Select(e => new PatientDto.PatientResponse(
            e.FirstName,
            e.LastName,
            e.Pesel,
            e.DateOfBirth,
            new AddressDto.NewAddress(
                e.Address.Street,
                e.Address.City,
                e.Address.PostalCode)))
            .ApplyPagination(pageNumber, pageSize)
            .ToList();
        
        
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Max(1, pageSize);
        var patientsCount =  patients.Count();
        
        return new PageResponse<PatientDto.PatientResponse>()
        {
             Data = data,
             PageNumber = pageNumber,
             PageSize = pageSize,
             TotalPages =  (int)Math.Ceiling((double)patientsCount / pageSize),
             TotalRecords =  patientsCount
        };
    }
}