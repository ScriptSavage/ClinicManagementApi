using ApplicationCore.Address.Dto;
using ApplicationCore.Doctor.Dto;
using ApplicationCore.Exceptions;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Medicine.Dto;
using ApplicationCore.Patient.Dto;
using ApplicationCore.Prescription.Dto;
using ApplicationCore.Visit.Dto;
using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Address;
using Infrastructure.Repositories.Doctor;
using Infrastructure.Repositories.Medicine;
using Infrastructure.Repositories.Patient;
using Infrastructure.Repositories.Prescription;
using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Patient.Service;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IPrescriptionRepository _prescriptionRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IMedicineRepository _medicineRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public PatientService(IPatientRepository patientRepository,
        IPrescriptionRepository prescriptionRepository,
        IDoctorRepository doctorRepository,
        IMedicineRepository medicineRepository,
        IAddressRepository addressRepository,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager)
    {
        _patientRepository = patientRepository;
        _prescriptionRepository = prescriptionRepository;
        _doctorRepository = doctorRepository;
        _medicineRepository = medicineRepository;
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
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

    public async Task UpdatePatientAsync(Guid patientId, PatientDto.UpdatePatientRequest request)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(patientId);
        
        if (patient is null)
        {
            throw new DoesNotExistsException("Patient not found");
        }

        if (patient.Address is null)
        {
            throw new DoesNotExistsException("Patient address not found");
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

    public async Task<IEnumerable<PatientDto.PatientPrescriptions>> GetPatientPrescriptionsAsync(string patientId)
    {
        var user = await _userManager.FindByIdAsync(patientId);
        if (user is null)
        {
            throw new DoesNotExistsException("User not found");
        }

        var patient = await _patientRepository.GetPatientByUserIdAsync(user.Id);

        if (patient is null)
        {
            throw new DoesNotExistsException("Patient not found");
        }
        
        var patientPrescriptions = await _prescriptionRepository
            .GetPrescriptionByPatientIdAsync(patient.PatientId);

        return patientPrescriptions.Select(e => new PatientDto.PatientPrescriptions(
            new DoctorDto.UpdateDoctorDto(e.Doctor.FirstName, 
                e.Doctor.LastName,
                e.Doctor.PWZ),
            new PrescriptionDto.Response(e.CreatedAt,
                e.Code),
            e.MedicinePrescriptions.Select(p=> new MedicineDto.Details(
                p.Medicine.Name,
                p.Medicine.ActiveSubstance,
                p.Medicine.PharmaceuticalForm,
                p.Dosage,
                p.Frequency,
                p.Instructions))));
    }

    public async Task<AddressDto.NewAddress> GetPatientAddresAsync(Guid patientId)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(patientId);

        if (patient is null)
        {
            throw new DoesNotExistsException("Patient not found");
        }
        
        var patientAddress = await _addressRepository.GetAddressByPatientId(patient.PatientId);
        
        if (patientAddress is null)
        {
            throw new DoesNotExistsException("Address not found");
        }

        return new AddressDto.NewAddress(
            patientAddress.Street,
            patientAddress.City,
            patientAddress.PostalCode);
        
    }

    public async Task UpdateMyData(string userId, PatientDto.UpdatePatientRequest request)
    {
       var user = await _userManager.FindByIdAsync(userId);

       if (user is null)
       {
           throw new DoesNotExistsException("User not found");
       }

       var patient = await _patientRepository.GetPatientByUserIdAsync(user.Id);

       if (patient is null)
       {
           throw new DoesNotExistsException("Patient not found");
       }

       var address = await _patientRepository.GetPatientAddressAsync(patient.PatientId);
       
       if (!string.IsNullOrWhiteSpace(request.FirstName)) patient.FirstName = request.FirstName;
       if (!string.IsNullOrWhiteSpace(request.LastName)) patient.LastName = request.LastName;
       if(!string.IsNullOrWhiteSpace(request.Address.Street)) address.Street = request.Address.Street;
       if(!string.IsNullOrWhiteSpace(request.Address.City)) address.City = request.Address.City;
       if(!string.IsNullOrWhiteSpace(request.Address.ZipCode)) address.PostalCode = request.Address.ZipCode;

       await _unitOfWork.SaveChangesAsync();
    }
}