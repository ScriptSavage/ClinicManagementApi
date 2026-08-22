using ApplicationCore.Doctor.Dto;
using ApplicationCore.Exceptions;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Patient.Dto;
using ApplicationCore.Visit.Dto;
using FluentValidation;
using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Doctor;
using Infrastructure.Repositories.Patient;
using Infrastructure.Repositories.Visit;
using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Visit.Service;

public class VisitService : IVisitService
{
    private readonly IVisitRepository _visitRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IValidator<VisitDto.CreateDescription> _completeVisitValidator;
    
    public VisitService(IVisitRepository visitRepository, 
        IDoctorRepository doctorRepository,
        IPatientRepository patientRepository,
        IValidator<VisitDto.CreateDescription> completeVisitValidator,
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork)
    {
        _visitRepository = visitRepository;
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
        _completeVisitValidator = completeVisitValidator;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<VisitDto.Response> CreateNewVisitAsync(string userId,VisitDto.CreateVisitRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            throw new DoesNotExistsException("User not found");
        }

        var patient = await _patientRepository.GetPatientByUserIdAsync(user.Id);
        
        

        var newVisit = new Infrastructure.Entities.Visit()
        {
            PatientId = patient.PatientId,
            DoctorId = request.DoctorId,
            Date = request.VisitDate,
        };
        
        
        
        await _visitRepository.AddNewVisitAsync(newVisit);
        await _unitOfWork.SaveChangesAsync();
        return new VisitDto.Response(newVisit.Date, null);
    }

    public async Task<VisitDto.VisitDetailsResponse> GetVisitDetailsAsync(Guid visitId)
    {
        var visit = await _visitRepository.GetVisitAsync(visitId);
        
        var doctor = await _doctorRepository.GetDoctorByIdAsync(visit.DoctorId);
        
        var patient = await _patientRepository.GetPatientByIdAsync(visit.PatientId);

        return new VisitDto.VisitDetailsResponse(new DoctorDto.UpdateDoctorDto(doctor.FirstName,
                doctor.LastName,
                doctor.PWZ),
            new PatientDto.Response(
                patient.FirstName,
                patient.LastName,
                patient.Pesel),
            visit.Date,
            visit.Description);
    }

    public async Task<IEnumerable<VisitDto.VisitDetailsResponse>> GetMyVisitsAsync(string userId)
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
        
        
        var patientVisits = await _visitRepository.GetVisitsByPatientIdAsync(patient.PatientId);
        
        return patientVisits.Select(e=> new VisitDto.VisitDetailsResponse(new DoctorDto.UpdateDoctorDto(e.Doctor.FirstName,
                e.Doctor.LastName,
                e.Doctor.PWZ),
            new PatientDto.Response(patient.FirstName,
                patient.LastName,
                patient.Pesel),
            e.Date,
            e.Description));
    }

    public async Task<PageResponse<VisitDto.VisitDetailsResponse>> GetVisitsDetailsAsync(int page, int pageSize)
    {
        var visits = (await _visitRepository.GetVisitsAsync()).AsQueryable();

        visits.ApplyPagination(page, pageSize);
        
        var projectionData = visits.Select(e=> new VisitDto.VisitDetailsResponse(
            new DoctorDto.UpdateDoctorDto(e.Doctor.FirstName,
                e.Doctor.LastName,
                e.Doctor.PWZ),
            new PatientDto.Response(e.Patient.FirstName,
                e.Patient.LastName,
                e.Patient.Pesel),
            e.Date,
            e.Description))
            .ToList();

        
        var totalRecords = visits.Count();
        page = Math.Max(1, page);
        pageSize = Math.Max(1, pageSize);

        return new PageResponse<VisitDto.VisitDetailsResponse>()
        {
            Data = projectionData,
            PageNumber = page,
            PageSize = pageSize,
            TotalPages = (int)(Math.Ceiling(totalRecords / (double)pageSize)),
            TotalRecords = totalRecords,
        };
    }

    public async Task CompleteVisitAsync(Guid visitId, VisitDto.CreateDescription dto)
    {
        var visit = await _visitRepository.GetVisitAsync(visitId);

        if (visit is null)
        {
            throw new DoesNotExistsException("Visit not found");
        }


        var validationResult = await _completeVisitValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        visit.Description = dto.Description;
        await _unitOfWork.SaveChangesAsync();
    }
}