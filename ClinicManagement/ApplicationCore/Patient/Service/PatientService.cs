using ApplicationCore.Address.Dto;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Patient.Dto;
using ApplicationCore.Visit.Dto;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Patient;

namespace ApplicationCore.Patient.Service;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IPatientRepository patientRepository,
        IUnitOfWork unitOfWork)
    {
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
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

    public async Task<PatientDto.PatientResponse> GetPatientByIdAsync(Guid id)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(id);

        if (patient is null)
        {
            throw new Exception("Patient not found");
        }

        var result = new PatientDto.PatientResponse(
            patient.FirstName, 
            patient.LastName, 
            patient.Pesel, 
            patient.DateOfBirth,
            new AddressDto.NewAddress(
                patient.Address.Street,
                patient.Address.City,
                patient.Address.PostalCode));
        
        return result;
    }

    public async Task UpdatePatientAsync(Guid id, PatientDto.UpdatePatientRequest request)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(id);
        if (patient is null)
        {
            throw new Exception("Patient not found");
        }
        
        if (!string.IsNullOrWhiteSpace(request.FirstName)) patient.FirstName = request.FirstName;
        if (!string.IsNullOrWhiteSpace(request.LastName)) patient.LastName = request.LastName;
        if(!string.IsNullOrWhiteSpace(request.Address.Street)) patient.Address.Street = request.Address.Street;
        if(!string.IsNullOrWhiteSpace(request.Address.City)) patient.Address.City = request.Address.City;
        if(!string.IsNullOrWhiteSpace(request.Address.ZipCode)) patient.Address.PostalCode = request.Address.ZipCode;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeletePatientAsync(Guid patientId)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(patientId);
        if (patient is null)
        {
            throw new Exception("Patient not found");
        }
        
        _patientRepository.DeletePatient(patient);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PatientDto.PatientVisitsResponse> GetPatientVisitsAsync(string patientId)
    {
        var id = Guid.Parse(patientId);
        var patient = await _patientRepository.GetPatientVisitsByIdAsync(id);
        
        if (patient is null)
        {
            throw new Exception("Patient not found");
        }
        
        
        var data = new PatientDto.PatientVisitsResponse(
            patient.FirstName, 
            patient.LastName,
            patient.Visits.Select(e=> new VisitDto.Response(
                e.Date,
                e.Description))
                .ToList()
            );
        
        return data;
    }
}